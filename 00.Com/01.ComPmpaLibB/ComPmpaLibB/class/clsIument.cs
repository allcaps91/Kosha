using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ComPmpaLibB
{
    public class clsIument
    {
        //clsIpdAcct cIAcct        = new clsIpdAcct();
        //clsIuSentChk cISC        = new clsIuSentChk();
        clsPmpaType.BonRate cBON = new clsPmpaType.BonRate();
        //DRG DRG                  = new DRG();

        /// <summary>
        /// IPD_NEW_MASTER 정보 저장
        /// <param name="strPano"></param>
        /// <param name="nIPDNO"></param>
        /// 2017-06-19 김민철
        /// </summary>
        /// <seealso cref="IUMENT.bas(Read_Ipd_Master 함수)"/>
        public string Read_Ipd_Master(PsmhDb pDbCon, string strPano, long nIPDNO)
        {
            DataTable Dt = new DataTable();
            clsPmpaFunc clsPF = new clsPmpaFunc();

            try
            {
                Dt = clsPF.Get_Ipd_New_Master(pDbCon, strPano, "", nIPDNO);

                if (Dt == null || Dt.Rows.Count == 0)
                {

                    clsPmpaType.IMST.MstCNT = 0; clsPmpaType.IMST.IPDNO = 0; clsPmpaType.IMST.Pano = "";
                    clsPmpaType.IMST.Sname = ""; clsPmpaType.IMST.Sex = ""; clsPmpaType.IMST.Age = 0;
                    clsPmpaType.IMST.AgeDays = 0;
                    clsPmpaType.IMST.Bi = ""; clsPmpaType.IMST.InDate = ""; clsPmpaType.IMST.InTime = "";
                    clsPmpaType.IMST.OutDate = ""; clsPmpaType.IMST.ActDate = ""; clsPmpaType.IMST.Ilsu = 0;
                    clsPmpaType.IMST.GbSTS = ""; clsPmpaType.IMST.DeptCode = ""; clsPmpaType.IMST.DrCode = "";
                    clsPmpaType.IMST.WardCode = ""; clsPmpaType.IMST.RoomCode = 0; clsPmpaType.IMST.PName = "";
                    clsPmpaType.IMST.GbSpc = ""; clsPmpaType.IMST.GbKekli = ""; clsPmpaType.IMST.GbGameK = "";
                    clsPmpaType.IMST.GbTewon = ""; clsPmpaType.IMST.Fee6 = 0; clsPmpaType.IMST.Bohun = "";
                    clsPmpaType.IMST.Jiyuk = ""; clsPmpaType.IMST.GelCode = ""; clsPmpaType.IMST.Religion = "";
                    clsPmpaType.IMST.GbCancer = ""; clsPmpaType.IMST.InOut = ""; clsPmpaType.IMST.Other = "";
                    clsPmpaType.IMST.GbDonggi = ""; clsPmpaType.IMST.OgPdBun = ""; clsPmpaType.IMST.OgPdBundtl = "";
                    clsPmpaType.IMST.Article = "";
                    clsPmpaType.IMST.JupboNo = ""; clsPmpaType.IMST.FromTrans = ""; clsPmpaType.IMST.ErAmt = 0;
                    clsPmpaType.IMST.ArcDate = ""; clsPmpaType.IMST.ArcQty = 0; clsPmpaType.IMST.GbOldSlip = false;
                    clsPmpaType.IMST.IcuQty = 0; clsPmpaType.IMST.Im180 = 0;
                    clsPmpaType.IMST.IllCode1 = ""; clsPmpaType.IMST.IllCode2 = ""; clsPmpaType.IMST.IllCode3 = "";
                    clsPmpaType.IMST.IllCode4 = ""; clsPmpaType.IMST.IllCode5 = ""; clsPmpaType.IMST.IllCode6 = "";
                    clsPmpaType.IMST.TrsDate = ""; clsPmpaType.IMST.Dept1 = ""; clsPmpaType.IMST.Dept2 = "";
                    clsPmpaType.IMST.Dept3 = ""; clsPmpaType.IMST.Doctor1 = ""; clsPmpaType.IMST.Doctor2 = "";
                    clsPmpaType.IMST.Doctor3 = ""; clsPmpaType.IMST.Ilsu1 = 0; clsPmpaType.IMST.Ilsu2 = 0;
                    clsPmpaType.IMST.Ilsu3 = 0;
                    clsPmpaType.IMST.Amset1 = " "; clsPmpaType.IMST.AmSet4 = " "; clsPmpaType.IMST.AmSet5 = "";
                    clsPmpaType.IMST.AmSet6 = " "; clsPmpaType.IMST.AmSet7 = " "; clsPmpaType.IMST.AmSet8 = "";
                    clsPmpaType.IMST.AmSet9 = ""; clsPmpaType.IMST.AmSetA = ""; clsPmpaType.IMST.MPano = "";
                    clsPmpaType.IMST.RDate = ""; clsPmpaType.IMST.TrsCNT = 0; clsPmpaType.IMST.LastTrs = 0;
                    clsPmpaType.IMST.IpwonTime = "";
                    clsPmpaType.IMST.CancelTime = "";
                    clsPmpaType.IMST.GatewonTime = "";
                    clsPmpaType.IMST.ROutDate = "";
                    clsPmpaType.IMST.SimsaTime = "";
                    clsPmpaType.IMST.PrintTime = "";
                    clsPmpaType.IMST.SunapTime = "";
                    clsPmpaType.IMST.GbCheckList = "";
                    clsPmpaType.IMST.MirBuildTime = "";
                    clsPmpaType.IMST.Remark = "";
                    clsPmpaType.IMST.GBSuDay = "";
                    clsPmpaType.IMST.PNEUMONIA = "";
                    clsPmpaType.IMST.Pregnant = "";
                    clsPmpaType.IMST.GbGoOut = "";
                    clsPmpaType.IMST.WardInTime = "";
                    clsPmpaType.IMST.TelRemark = "";
                    clsPmpaType.IMST.GbExam = "";
                    clsPmpaType.IMST.Secret = "";  //2012-11-22
                    clsPmpaType.IMST.DrgCode = "";
                    clsPmpaType.IMST.KTASLEVL = "";          //2015-12-28
                    clsPmpaType.IMST.FROOM = "";
                    clsPmpaType.IMST.FROOMETC = "";
                    clsPmpaType.IMST.GBJIWON = "";       //2016-07-14
                    clsPmpaType.IMST.T_CARE = "";        //2016-07-19
                    clsPmpaType.IMST.Pass_Info = "";
                    clsPmpaType.IMST.RETURN_HOSP = "";
                    clsPmpaType.IMST.KTAS_HIS = "";

                    Dt.Dispose();
                    Dt = null;

                    return "NO";
                }
                else
                {
                    clsPmpaType.IMST.MstCNT = Dt.Rows.Count;  //자료의 건수

                    clsPmpaType.IMST.IPDNO = Convert.ToInt32(Dt.Rows[0]["IPDNO"].ToString());
                    clsPmpaType.IMST.Pano = Dt.Rows[0]["Pano"].ToString().Trim();
                    clsPmpaType.IMST.Sname = Dt.Rows[0]["Sname"].ToString().Trim();
                    clsPmpaType.IMST.Sex = Dt.Rows[0]["Sex"].ToString().Trim();
                    clsPmpaType.IMST.Age = Convert.ToInt16(Dt.Rows[0]["Age"].ToString());
                    clsPmpaType.IMST.Bi = Dt.Rows[0]["Bi"].ToString().Trim();
                    clsPmpaType.IMST.InDate = VB.Left(Dt.Rows[0]["InDate"].ToString().Trim(), 10);
                    clsPmpaType.IMST.InTime = VB.Right(Dt.Rows[0]["InDate"].ToString().Trim(), 5);
                    clsPmpaType.IMST.OutDate = Dt.Rows[0]["OutDate"].ToString().Trim();
                    clsPmpaType.IMST.ActDate = Dt.Rows[0]["ActDate"].ToString().Trim();
                    clsPmpaType.IMST.Ilsu = Convert.ToInt16(Dt.Rows[0]["Ilsu"].ToString());
                    clsPmpaType.IMST.GbSTS = Dt.Rows[0]["GbSTS"].ToString().Trim();
                    clsPmpaType.IMST.DeptCode = Dt.Rows[0]["DeptCode"].ToString().Trim();
                    clsPmpaType.IMST.DrCode = Dt.Rows[0]["DrCode"].ToString().Trim();
                    clsPmpaType.IMST.WardCode = Dt.Rows[0]["WardCode"].ToString().Trim();
                    clsPmpaType.IMST.RoomCode = Convert.ToInt16(Dt.Rows[0]["RoomCode"].ToString());
                    clsPmpaType.IMST.PName = Dt.Rows[0]["Pname"].ToString().Trim();
                    clsPmpaType.IMST.GbSpc = Dt.Rows[0]["GbSpc"].ToString().Trim();
                    clsPmpaType.IMST.GbKekli = Dt.Rows[0]["GbKekli"].ToString().Trim();
                    clsPmpaType.IMST.GbGameK = Dt.Rows[0]["GbGamek"].ToString().Trim();
                    clsPmpaType.IMST.GbTewon = Dt.Rows[0]["GbTewon"].ToString().Trim();
                    clsPmpaType.IMST.Fee6 = Convert.ToInt16(Dt.Rows[0]["Fee6"].ToString());
                    clsPmpaType.IMST.Bohun = Dt.Rows[0]["Bohun"].ToString().Trim();
                    clsPmpaType.IMST.Jiyuk = Dt.Rows[0]["Jiyuk"].ToString().Trim();
                    clsPmpaType.IMST.GelCode = Dt.Rows[0]["GelCode"].ToString().Trim();
                    clsPmpaType.IMST.Religion = Dt.Rows[0]["Religion"].ToString().Trim();
                    clsPmpaType.IMST.GbCancer = Dt.Rows[0]["GbCancer"].ToString().Trim();
                    clsPmpaType.IMST.InOut = Dt.Rows[0]["InOut"].ToString().Trim();
                    clsPmpaType.IMST.Other = Dt.Rows[0]["Other"].ToString().Trim();
                    clsPmpaType.IMST.GbDonggi = Dt.Rows[0]["GbDonggi"].ToString().Trim();
                    clsPmpaType.IMST.OgPdBun = Dt.Rows[0]["OgPdBun"].ToString().Trim();
                    clsPmpaType.IMST.OgPdBundtl = Dt.Rows[0]["OgPdBundtl"].ToString().Trim();
                    clsPmpaType.IMST.Article = Dt.Rows[0]["Article"].ToString().Trim();
                    clsPmpaType.IMST.JupboNo = Dt.Rows[0]["JupboNo"].ToString().Trim();
                    clsPmpaType.IMST.FromTrans = Dt.Rows[0]["FromTrans"].ToString().Trim();
                    clsPmpaType.IMST.ErAmt = Convert.ToInt32(Dt.Rows[0]["ErAmt"].ToString());
                    clsPmpaType.IMST.ArcDate = Dt.Rows[0]["ArcDate"].ToString().Trim();
                    clsPmpaType.IMST.ArcQty = Convert.ToInt16(Dt.Rows[0]["ArcQty"].ToString());
                    clsPmpaType.IMST.GbOldSlip = Dt.Rows[0]["GbOldSlip"].ToString().Trim() == "Y" ? true : false;
                    clsPmpaType.IMST.IllCode1 = Dt.Rows[0]["IllCode1"].ToString().Trim();
                    clsPmpaType.IMST.IllCode2 = Dt.Rows[0]["IllCode2"].ToString().Trim();
                    clsPmpaType.IMST.IllCode3 = Dt.Rows[0]["IllCode3"].ToString().Trim();
                    clsPmpaType.IMST.IllCode4 = Dt.Rows[0]["IllCode4"].ToString().Trim();
                    clsPmpaType.IMST.IllCode5 = Dt.Rows[0]["IllCode5"].ToString().Trim();
                    clsPmpaType.IMST.IllCode6 = Dt.Rows[0]["IllCode6"].ToString().Trim();
                    clsPmpaType.IMST.TrsDate = Dt.Rows[0]["TrsDate"].ToString().Trim();
                    clsPmpaType.IMST.Dept1 = Dt.Rows[0]["Dept1"].ToString().Trim();
                    clsPmpaType.IMST.Dept2 = Dt.Rows[0]["Dept2"].ToString().Trim();
                    clsPmpaType.IMST.Dept3 = Dt.Rows[0]["Dept3"].ToString().Trim();
                    clsPmpaType.IMST.Doctor1 = Dt.Rows[0]["Doctor1"].ToString().Trim();
                    clsPmpaType.IMST.Doctor2 = Dt.Rows[0]["Doctor2"].ToString().Trim();
                    clsPmpaType.IMST.Doctor3 = Dt.Rows[0]["Doctor3"].ToString().Trim();
                    clsPmpaType.IMST.Ilsu1 = Convert.ToInt16(VB.Val(Dt.Rows[0]["Ilsu1"].ToString()));
                    clsPmpaType.IMST.Ilsu2 = Convert.ToInt16(VB.Val(Dt.Rows[0]["Ilsu2"].ToString()));
                    clsPmpaType.IMST.Ilsu3 = Convert.ToInt16(VB.Val(Dt.Rows[0]["Ilsu3"].ToString()));
                    clsPmpaType.IMST.Amset1 = Dt.Rows[0]["AmSet1"].ToString().Trim();
                    clsPmpaType.IMST.AmSet4 = Dt.Rows[0]["AmSet4"].ToString().Trim();
                    clsPmpaType.IMST.AmSet5 = Dt.Rows[0]["AmSet5"].ToString().Trim();
                    clsPmpaType.IMST.AmSet6 = Dt.Rows[0]["AmSet6"].ToString().Trim();
                    clsPmpaType.IMST.AmSet7 = Dt.Rows[0]["AmSet7"].ToString().Trim();
                    clsPmpaType.IMST.AmSet8 = Dt.Rows[0]["AmSet8"].ToString().Trim();
                    clsPmpaType.IMST.AmSet9 = Dt.Rows[0]["AmSet9"].ToString().Trim();
                    clsPmpaType.IMST.AmSetA = Dt.Rows[0]["AmSetA"].ToString().Trim();
                    if (clsPmpaType.IMST.AmSet4 == "3")
                    {
                        clsPmpaType.IMST.MPano = Dt.Rows[0]["JupboNo"].ToString().Trim();
                    }
                    else
                    {
                        clsPmpaType.IMST.MPano = "";
                    }

                    clsPmpaType.IMST.RDate = Dt.Rows[0]["RDate"].ToString().Trim();
                    clsPmpaType.IMST.TrsCNT = Convert.ToInt16(Dt.Rows[0]["TrsCNT"].ToString());
                    clsPmpaType.IMST.LastTrs = Convert.ToInt32(Dt.Rows[0]["LastTrs"].ToString());
                    clsPmpaType.IMST.IpwonTime = Dt.Rows[0]["IpwonTime"].ToString().Trim();
                    clsPmpaType.IMST.CancelTime = Dt.Rows[0]["CancelTime"].ToString().Trim();
                    clsPmpaType.IMST.GatewonTime = Dt.Rows[0]["GatewonTime"].ToString().Trim();
                    clsPmpaType.IMST.ROutDate = Dt.Rows[0]["ROutDate"].ToString().Trim();
                    clsPmpaType.IMST.SimsaTime = Dt.Rows[0]["SimsaTime"].ToString().Trim();
                    clsPmpaType.IMST.PrintTime = Dt.Rows[0]["PrintTime"].ToString().Trim();
                    clsPmpaType.IMST.SunapTime = Dt.Rows[0]["SunapTime"].ToString().Trim();
                    clsPmpaType.IMST.GbCheckList = Dt.Rows[0]["GbCheckList"].ToString().Trim();
                    clsPmpaType.IMST.MirBuildTime = Dt.Rows[0]["MirBuildTime"].ToString().Trim();
                    clsPmpaType.IMST.Remark = Dt.Rows[0]["Remark"].ToString().Trim();
                    clsPmpaType.IMST.GBSuDay = Dt.Rows[0]["GBSUDAY"].ToString().Trim();
                    clsPmpaType.IMST.PNEUMONIA = Dt.Rows[0]["PNEUMONIA"].ToString().Trim();
                    clsPmpaType.IMST.Pregnant = Dt.Rows[0]["pregnant"].ToString().Trim();
                    clsPmpaType.IMST.GbGoOut = Dt.Rows[0]["GbGoOut"].ToString().Trim();
                    clsPmpaType.IMST.WardInTime = Dt.Rows[0]["WardInTime"].ToString().Trim();
                    clsPmpaType.IMST.TelRemark = Dt.Rows[0]["TelRemark"].ToString().Trim();
                    clsPmpaType.IMST.GbExam = Dt.Rows[0]["GbExam"].ToString().Trim();
                    clsPmpaType.IMST.Secret = Dt.Rows[0]["Secret"].ToString().Trim();  //2012-11-22
                    clsPmpaType.IMST.GbDRG = Dt.Rows[0]["GBDRG"].ToString().Trim();    //2013-06-30
                    clsPmpaType.IMST.DrgCode = Dt.Rows[0]["DRGCODE"].ToString().Trim();   //2013-06-30
                    clsPmpaType.IMST.KTASLEVL = Dt.Rows[0]["KTASLEVL"].ToString().Trim();  //2015-12-28
                    clsPmpaType.IMST.FROOM = Dt.Rows[0]["FROOM"].ToString().Trim();
                    clsPmpaType.IMST.FROOMETC = Dt.Rows[0]["FROOMETC"].ToString().Trim();
                    clsPmpaType.IMST.GBJIWON = Dt.Rows[0]["GBJIWON"].ToString().Trim();     //2016-07-14
                    clsPmpaType.IMST.T_CARE = Dt.Rows[0]["T_CARE"].ToString().Trim();       //2016-07-19
                    clsPmpaType.IMST.RETURN_HOSP = Dt.Rows[0]["RETUN_HOSP"].ToString().Trim();
                    clsPmpaType.IMST.Pass_Info = Dt.Rows[0]["PASS_INFO"].ToString().Trim();
                    clsPmpaType.IMST.KTAS_HIS = Dt.Rows[0]["KTAS_HIS"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                return "OK";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "NO";
            }
        }

        /// <summary>
        /// IPD_TRANS 정보 저장, IPD_NEW_MASTER
        /// <param name="strPano"></param>
        /// <param name="nTrsNo">TRSNO</param>
        /// 2017-06-19 김민철
        /// READ_IPD_TRANS 함수와 병합
        /// </summary>
        /// <seealso
        /// 
        /// cref="IUMENT.bas(READ_IPD_TRANS_NEW 함수)"/>
        public void Read_Ipd_Mst_Trans(PsmhDb pDbCon, string strPano, long nTrsNo, string strTemp)
        {
            clsPmpaFunc cPF = new clsPmpaFunc();
            clsIpdAcct cIAcct = new clsIpdAcct();
            ComFunc CF = new ComFunc();

            string strBDate = string.Empty;
            string strJuminNo = string.Empty;
            string strChild = string.Empty;
            string strMCode = string.Empty;
            string strOgPdBun = string.Empty;
            string strDept = string.Empty;

            int i = 0;

            try
            {
                Read_Ipd_Trans_Clear();

                DataTable Dt = cPF.Get_Ipd_Mst_Trs(pDbCon, strPano, nTrsNo, strTemp);

                clsPmpaType.TIT.MstCNT = Dt.Rows.Count;

                if (Dt == null || Dt.Rows.Count == 0)
                {
                    return;
                }
                else
                {
                    if (nTrsNo > 0) { clsPmpaType.TIT.Trsno = nTrsNo; }
                    else { clsPmpaType.TIT.Trsno = Convert.ToUInt32(VB.Val(Dt.Rows[0]["TRSNO"].ToString())); }

                    clsPmpaType.TIT.Ipdno = Convert.ToUInt32(Dt.Rows[0]["IPDNO"].ToString());
                    clsPmpaType.TIT.Pano = Dt.Rows[0]["PANO"].ToString();
                    clsPmpaType.TIT.GbIpd = Dt.Rows[0]["GBIPD"].ToString();
                    clsPmpaType.TIT.TGbSts = Dt.Rows[0]["TGbSts"].ToString();
                    clsPmpaType.TIT.MGbSts = Dt.Rows[0]["MGbSts"].ToString();
                    clsPmpaType.TIT.WardCode = Dt.Rows[0]["WardCode"].ToString();
                    clsPmpaType.TIT.RoomCode = Dt.Rows[0]["RoomCode"].ToString();
                    clsPmpaType.TIT.Bi = Dt.Rows[0]["BI"].ToString();
                    clsPmpaType.TIT.Sname = Dt.Rows[0]["SName"].ToString();
                    clsPmpaType.TIT.Sex = Dt.Rows[0]["Sex"].ToString();
                    clsPmpaType.TIT.Age = Convert.ToUInt16(Dt.Rows[0]["Age"].ToString());
                    clsPmpaType.TIT.Jumin1 = Dt.Rows[0]["JUMIN1"].ToString();
                    clsPmpaType.TIT.Jumin2 = Dt.Rows[0]["JUMIN2"].ToString();
                    clsPmpaType.TIT.Jumin3 = clsAES.DeAES(Dt.Rows[0]["JUMIN3"].ToString());
                    clsPmpaType.TIT.InDate = Dt.Rows[0]["INDATE"].ToString();
                    clsPmpaType.TIT.OutDate = Dt.Rows[0]["OUTDATE"].ToString();
                    clsPmpaType.TIT.AgeDays = ComFunc.AgeCalcEx_Zero(clsPmpaType.TIT.Jumin1 + clsPmpaType.TIT.Jumin3, clsPmpaType.TIT.InDate);
                    clsPmpaType.TIT.ActDate = Dt.Rows[0]["ACTDATE"].ToString();
                    clsPmpaType.TIT.DeptCode = Dt.Rows[0]["DEPTCODE"].ToString();
                    clsPmpaType.TIT.DrCode = Dt.Rows[0]["DRCODE"].ToString();
                    clsPmpaType.TIT.Ilsu = Convert.ToUInt16(Dt.Rows[0]["ILSU"].ToString());
                    clsPmpaType.TIT.Bi = Dt.Rows[0]["BI"].ToString();
                    clsPmpaType.TIT.Kiho = Dt.Rows[0]["KIHO"].ToString();
                    clsPmpaType.TIT.GKiho = Dt.Rows[0]["GKIHO"].ToString();
                    clsPmpaType.TIT.PName = Dt.Rows[0]["PNAME"].ToString();
                    clsPmpaType.TIT.Gwange = Dt.Rows[0]["GWANGE"].ToString();
                    clsPmpaType.TIT.BonRate = Convert.ToUInt16(Dt.Rows[0]["BONRATE"].ToString());
                    clsPmpaType.TIT.GISULRATE = Convert.ToUInt16(Dt.Rows[0]["GISULRATE"].ToString());
                    clsPmpaType.TIT.GbGameK = Dt.Rows[0]["GBGAMEK"].ToString();
                    clsPmpaType.TIT.Bohun = Dt.Rows[0]["BOHUN"].ToString();
                    clsPmpaType.TIT.Amset1 = Dt.Rows[0]["AMSET1"].ToString();
                    clsPmpaType.TIT.AmSet2 = Dt.Rows[0]["AMSET2"].ToString();
                    clsPmpaType.TIT.AmSet3 = Dt.Rows[0]["AMSET3"].ToString();
                    clsPmpaType.TIT.AmSet4 = Dt.Rows[0]["AMSET4"].ToString();
                    clsPmpaType.TIT.AmSet5 = Dt.Rows[0]["AMSET5"].ToString();
                    clsPmpaType.TIT.AmSetB = Dt.Rows[0]["AMSETB"].ToString();
                    clsPmpaType.TIT.IllCode1 = Dt.Rows[0]["IllCode1"].ToString();
                    clsPmpaType.TIT.IllCode2 = Dt.Rows[0]["IllCode2"].ToString();
                    clsPmpaType.TIT.IllCode3 = Dt.Rows[0]["IllCode3"].ToString();
                    clsPmpaType.TIT.IllCode4 = Dt.Rows[0]["IllCode4"].ToString();
                    clsPmpaType.TIT.IllCode5 = Dt.Rows[0]["IllCode5"].ToString();
                    clsPmpaType.TIT.IllCode6 = Dt.Rows[0]["IllCode6"].ToString();
                    clsPmpaType.TIT.FromTrans = Dt.Rows[0]["FROMTRANS"].ToString();
                    clsPmpaType.TIT.ErAmt = Convert.ToUInt32(VB.Val(Dt.Rows[0]["ERAMT"].ToString()));
                    clsPmpaType.TIT.DtGamek = Convert.ToUInt32(VB.Val(Dt.Rows[0]["DTGAMEK"].ToString()));
                    clsPmpaType.TIT.JupboNo = Dt.Rows[0]["JUPBONO"].ToString();
                    clsPmpaType.TIT.GbDRG = Dt.Rows[0]["GBDRG"].ToString();
                    clsPmpaType.TIT.DrgWRTNO = Convert.ToUInt32(VB.Val(Dt.Rows[0]["DRGWRTNO"].ToString()));
                    clsPmpaType.TIT.SangAmt = Convert.ToUInt32(VB.Val(Dt.Rows[0]["SANGAMT"].ToString()));
                    clsPmpaType.TIT.OgPdBun = Dt.Rows[0]["OGPDBUN"].ToString();
                    clsPmpaType.TIT.OgPdBun2 = Dt.Rows[0]["OGPDBUN2"].ToString();
                    clsPmpaType.TIT.OgPdBundtl = Dt.Rows[0]["OGPDBUNdtl"].ToString();
                    clsPmpaType.TIT.GelCode = Dt.Rows[0]["GelCode"].ToString();
                    clsPmpaType.TIT.VCode = Dt.Rows[0]["Vcode"].ToString();
                    clsPmpaType.TIT.JSIM_REMARK = Dt.Rows[0]["JSIM_REMARK"].ToString();
                    clsPmpaType.TIT.JSIM_REMARK9 = Dt.Rows[0]["JSIM_REMARK9"].ToString();
                    clsPmpaType.TIT.Gbilban2 = Dt.Rows[0]["Gbilban2"].ToString();                  //외국new
                    clsPmpaType.TIT.GbSpc = Dt.Rows[0]["GbSPC"].ToString();                        //선택진료
                                                                                                   // IPD_MASTER
                    clsPmpaType.TIT.Sname = Dt.Rows[0]["SName"].ToString();
                    clsPmpaType.TIT.Age = Convert.ToUInt16(Dt.Rows[0]["Age"].ToString());
                    clsPmpaType.TIT.Sex = Dt.Rows[0]["Sex"].ToString();
                    clsPmpaType.TIT.MGbSts = Dt.Rows[0]["MGbSts"].ToString();
                    clsPmpaType.TIT.WardCode = Dt.Rows[0]["WardCode"].ToString();
                    clsPmpaType.TIT.RoomCode = Dt.Rows[0]["RoomCode"].ToString();
                    clsPmpaType.TIT.M_InDate = Dt.Rows[0]["M_InDate"].ToString();
                    clsPmpaType.TIT.M_OutDate = Dt.Rows[0]["M_OutDate"].ToString();
                    clsPmpaType.TIT.M_ActDate = Dt.Rows[0]["M_ActDate"].ToString();
                    clsPmpaType.TIT.M_Ilsu = Convert.ToUInt16(Dt.Rows[0]["M_Ilsu"].ToString());
                    clsPmpaType.TIT.MiIlsu = Convert.ToUInt16(VB.Val(Dt.Rows[0]["MiIlsu"].ToString()));
                    clsPmpaType.TIT.MiArcDate = Dt.Rows[0]["MIARCDATE"].ToString();
                    clsPmpaType.TIT.M_GBSuDay = Dt.Rows[0]["GbSuDay"].ToString();
                    clsPmpaType.TIT.DrgCode = Dt.Rows[0]["DrgCode"].ToString();
                    clsPmpaType.TIT.ArcDate = Dt.Rows[0]["ArcDate"].ToString();
                    clsPmpaType.TIT.ArcQty = Convert.ToUInt16(Dt.Rows[0]["ArcQty"].ToString());
                    clsPmpaType.TIT.Fee6 = Convert.ToUInt16(Dt.Rows[0]["Fee6"].ToString());
                    clsPmpaType.TIT.GbKekli = Dt.Rows[0]["GbKekli"].ToString();
                    clsPmpaType.TIT.IcuQty = Convert.ToUInt16(Dt.Rows[0]["IcuQty"].ToString());
                    clsPmpaType.TIT.GbTewon = Dt.Rows[0]["GbTewon"].ToString();                    //퇴원구분
                    clsPmpaType.TIT.ROutDate = Dt.Rows[0]["ROUTDATE"].ToString();
                    clsPmpaType.TIT.SimsaTime = Dt.Rows[0]["SIMSATIME"].ToString();
                    clsPmpaType.TIT.PrintTime = Dt.Rows[0]["PRINTTIME"].ToString();
                    clsPmpaType.TIT.SunapTime = Dt.Rows[0]["SUNAPTIME"].ToString();
                    clsPmpaType.TIT.GbCheckList = Dt.Rows[0]["GBCHECKLIST"].ToString();
                    clsPmpaType.TIT.MirBuildTime = Dt.Rows[0]["MIRBUILDTIME"].ToString();
                    clsPmpaType.TIT.TGbSts = Dt.Rows[0]["TGbSts"].ToString();
                    //clsPmpaType.TIT.VCode = Dt.Rows[0]["Vcode"].ToString();
                    clsPmpaType.TIT.GbSang = Dt.Rows[0]["GBSANG"].ToString();
                    // 재원심사--------------------------------------------------------------------
                    clsPmpaType.JSIM.InDate = Dt.Rows[0]["INDATE"].ToString(); 
                    clsPmpaType.JSIM.LDATE = Dt.Rows[0]["JSIM_LDATE"].ToString().Trim();
                    if (clsPmpaType.JSIM.LDATE == "")
                    {
                        clsPmpaType.JSIM.LDATE = Dt.Rows[0]["INDATE"].ToString().Trim();
                    }
                    clsPmpaType.JSIM.TODATE = CF.DATE_ADD(pDbCon, clsPublic.GstrSysDate, -1);

                    clsPmpaType.TIT.JSIM_LDATE = Dt.Rows[0]["JSIM_LDATE"].ToString();
                    clsPmpaType.TIT.JSIM_SABUN = Dt.Rows[0]["JSIM_SABUN"].ToString();
                    clsPmpaType.TIT.JSIM_Set = Dt.Rows[0]["JSIM_SET"].ToString();
                    clsPmpaType.TIT.JSIM_OK = Dt.Rows[0]["JSIM_OK"].ToString();
                    clsPmpaType.TIT.FCode = Dt.Rows[0]["FCODE"].ToString();
                    clsPmpaType.TIT.KTASLEVL = Dt.Rows[0]["KTASLEVL"].ToString(); //2015-12-28
                    clsPmpaType.TIT.T_CARE = Dt.Rows[0]["T_CARE"].ToString(); //2015-12-28
                    clsPmpaType.TIT.TROWID = Dt.Rows[0]["TROWID"].ToString();
                    clsPmpaType.TIT.DRGOG = Dt.Rows[0]["DRGOG"].ToString();
                    clsPmpaType.TIT.BirthDay = Dt.Rows[0]["Birth"].ToString();
                    clsPmpaType.TIT.GBHU = Dt.Rows[0]["GBHU"].ToString(); 
                    for (i = 1; i < 96; i++)
                    {
                        clsPmpaType.TIT.Amt[i] = Convert.ToInt32(VB.Val(Dt.Rows[0]["AMT" + VB.Format(i, "00")].ToString()));
                        if (i < 21)
                        {
                            clsPmpaType.TIT.TotGub += clsPmpaType.TIT.Amt[i];
                        }
                        else if (i > 20 && i < 50)
                        {
                            clsPmpaType.TIT.TotBigub += clsPmpaType.TIT.Amt[i];
                        }
                    }
                }

                Dt.Dispose();
                Dt = null;

                #region //입원 본인부담율 세팅
                //기준일자 세팅
                cBON.BI = clsPmpaType.TIT.Bi;
                cBON.SDATE = clsPmpaType.TIT.InDate;
                strJuminNo = clsPmpaType.TIT.Jumin1 + clsPmpaType.TIT.Jumin3;
                cBON.IO = "I";
                //나이구분 체크
                cBON.CHILD = cPF.Acct_Age_Gubun(clsPmpaType.TIT.Age, strJuminNo, clsPmpaType.TIT.InDate, cBON.IO);
                //cBON.MCODE = cPF.Rtn_Ipd_OgPdBunDtl(clsPmpaType.TIT.OgPdBun);
                if (clsPmpaType.TIT.OgPdBun == "1")
                {
                    cBON.MCODE = "E000";
                    cBON.VCODE = "EV00";
                }
                else if (clsPmpaType.TIT.OgPdBun == "2")
                {
                    cBON.MCODE = "F000";
                    cBON.VCODE = "EV00";
                }
                else
                {
                    cBON.MCODE = cPF.Rtn_Ipd_OgPdBunDtl(clsPmpaType.TIT.OgPdBun);
                    cBON.VCODE = clsPmpaType.TIT.VCode;
                }
                
                if (cPF.Rtn_Ipd_OgPdBunDtl(clsPmpaType.TIT.OgPdBun) == "")
                {
                    cBON.OGPDBUN = clsPmpaType.TIT.OgPdBun;
                }
                else
                {
                    cBON.OGPDBUN = clsPmpaType.TIT.OgPdBundtl;
                }
                cBON.DEPT = clsPmpaType.TIT.DeptCode;
                cBON.FCODE = clsPmpaType.TIT.FCode;

                //기본 부담율 계산
                if (cIAcct.Read_IBon_Rate(pDbCon, cBON) == false)
                {
                    clsAlert cA = new ComPmpaLibB.clsAlert();
                    cA.Alert_BonRate(cBON);
                    return;
                }

                if (clsPmpaType.TIT.Bohun == "3")   //장애인은 본인부담율 재조정 
                {
                    if ((cBON.BI == "11" && cBON.MCODE == "F000") || cBON.BI == "22")
                    {
                        clsPmpaType.IBR.Jin = 0;
                        clsPmpaType.IBR.Bohum = 0;
                        clsPmpaType.IBR.CTMRI = 0;
                    }
                }

                #endregion
                cPF = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                cPF = null;
                return;
            }
        }

        /// <summary>
        /// 입원정보 세팅 : 연말정산용
        /// 박웅규
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgTRSNO"></param>
        /// <param name="ArgIPDNO"></param>
        /// <seealso cref="IUMENT.bas(READ_IPD_TRANS)"/>
        public void READ_IPD_TRANS_Junsan(PsmhDb pDbCon, long ArgTRSNO, long ArgIPDNO = 0)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //변수를 Clear
            Read_Ipd_Trans_Clear();
            clsPmpaType.TIT.Ipdno = 0;
            clsPmpaType.TIT.Trsno = 0;
            clsPmpaType.TIT.Pano = "";
	        clsPmpaType.TIT.Sname = "";
            clsPmpaType.TIT.TotGub = 0;
            clsPmpaType.TIT.TotBigub = 0;
            clsPmpaType.TIT.Gbilban2 = "";
            clsPmpaType.TIT.GbSpc = "";
            clsPmpaType.TIT.JinDtl = "";
            clsPmpaType.TIT.OgPdBun2 = "";
            clsPmpaType.TIT.M_GBSuDay = "";

            clsPmpaType.TIT.GbDRG = "";
            clsPmpaType.TIT.DrgCode = "";
            clsPmpaType.TIT.GbTax = "";
            clsPmpaType.TIT.FCode = "";

            for (i = 1; i <= 65; i++)
            {
                clsPmpaType.TIT.Amt[i] = 0;
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT a.TRSNO,a.IPDNO,a.PANO,a.GBIPD,a.DEPTCODE,a.DRCODE,a.ILSU,";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(a.INDATE,'YYYY-MM-DD') InDate,";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(a.OUTDATE,'YYYY-MM-DD') OutDate,";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(a.ACTDATE,'YYYY-MM-DD') ActDate, ";
            SQL = SQL + ComNum.VBLF + "       a.BI,a.KIHO,a.GKIHO,a.PNAME,a.GWANGE,a.BONRATE,a.GISULRATE,";
            SQL = SQL + ComNum.VBLF + "       a.AMSET1,a.AMSET2,a.AMSET3,a.AMSET4,a.AMSET5,a.AMSETB,a.FROMTRANS,a.ERAMT,";
            SQL = SQL + ComNum.VBLF + "       a.JUPBONO,a.GBDRG,a.DRGWRTNO,a.SANGAMT,a.DTGAMEK,a.OGPDBUN, ";
            SQL = SQL + ComNum.VBLF + "       a.IllCode1,a.IllCode2,a.IllCode3,a.IllCode4,a.IllCode5,a.IllCode6,";
            SQL = SQL + ComNum.VBLF + "       b.SName,b.Age,b.Sex,b.GbSTS,b.WardCode,b.RoomCode,a.GBGAMEK,a.BOHUN,a.JinDtl,";
            SQL = SQL + ComNum.VBLF + "       b.GbSuDay,";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(b.INDATE,'YYYY-MM-DD') M_InDate, ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(b.OUTDATE,'YYYY-MM-DD') M_OutDate,";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(b.ACTDATE,'YYYY-MM-DD') M_ActDate,";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(b.ArcDate,'YYYY-MM-DD') ArcDate,";
            SQL = SQL + ComNum.VBLF + "       b.Ilsu M_Ilsu,b.Fee6,b.ArcQty,b.GbKekli,b.IcuQty,b.IcuQty2,A.GelCode,b.GbTewon, ";
            SQL = SQL + ComNum.VBLF + "       a.AMT01,a.AMT02,a.AMT03,a.AMT04,a.AMT05,a.AMT06,a.AMT07,a.AMT08,a.AMT09,a.AMT10,";
            SQL = SQL + ComNum.VBLF + "       a.AMT11,a.AMT12,a.AMT13,a.AMT14,a.AMT15,a.AMT16,a.AMT17,a.AMT18,a.AMT19,a.AMT20,";
            SQL = SQL + ComNum.VBLF + "       a.AMT21,a.AMT22,a.AMT23,a.AMT24,a.AMT25,a.AMT26,a.AMT27,a.AMT28,a.AMT29,a.AMT30,";
            SQL = SQL + ComNum.VBLF + "       a.AMT31,a.AMT32,a.AMT33,a.AMT34,a.AMT35,a.AMT36,a.AMT37,a.AMT38,a.AMT39,a.AMT40,";
            SQL = SQL + ComNum.VBLF + "       a.AMT41,a.AMT42,a.AMT43,a.AMT44,a.AMT45,a.AMT46,a.AMT47,a.AMT48,a.AMT49,a.AMT50,";
            SQL = SQL + ComNum.VBLF + "       a.AMT51,a.AMT52,a.AMT53,a.AMT54,a.AMT55,a.AMT56,a.AMT57,a.AMT58,a.AMT59,a.AMT60,";
            SQL = SQL + ComNum.VBLF + "       a.AMT61,a.AMT62,a.AMT63,a.AMT64, ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.ROUTDATE,'YYYY-MM-DD HH24;MI') ROUTDATE,                               ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.SIMSATIME,'YYYY-MM-DD HH24;MI') SIMSATIME,                             ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.PRINTTIME,'YYYY-MM-DD HH24;MI') PRINTTIME,                             ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.SUNAPTIME,'YYYY-MM-DD HH24;MI') SUNAPTIME,                             ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.MIRBUILDTIME,'YYYY-MM-DD HH24;MI') MIRBUILDTIME,                       ";
            SQL = SQL + ComNum.VBLF + "       A.GBCHECKLIST,A.GBSTS, A.VCODE, A.GBSANG,A.MSEQNO,a.OGPDBUNdtl,a.OgPdBun2,a.Gbilban2,a.GbSPC,a.DrgCode, ";
            SQL = SQL + ComNum.VBLF + "       A.DRGADC1,A.DRGADC2,A.DRGADC3,A.DRGADC4,A.DRGADC5,                     ";
            SQL = SQL + ComNum.VBLF + "       b.MiIlsu, TO_CHAR(b.MiArcDate,'YYYY-MM-DD') MiArcDate,                              ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.JSIM_LDATE,'YYYY-MM-DD') JSIM_LDATE  , A.JSIM_SABUN , A.JSIM_SET, A.JSIM_OK, ";
            SQL = SQL + ComNum.VBLF + "       a.Amt67,a.GbTax, a.FCODE,b.KTASLEVL, b.T_CARE ,a.GBHU ";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.IPD_TRANS a,ADMIN.IPD_NEW_MASTER b ";
            SQL = SQL + ComNum.VBLF + " WHERE a.TRSNO = " + ArgTRSNO + " ";
            if (ArgIPDNO > 0)
            {
                SQL = SQL + ComNum.VBLF + "  AND a.IPDNO = " + ArgIPDNO;
            }
            SQL = SQL + ComNum.VBLF + "   AND a.IPDNO = b.IPDNO(+) ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                clsPmpaType.TIT.Ipdno = 0;
                clsPmpaType.TIT.Trsno = 0;
                clsPmpaType.TIT.Pano = "";
                clsPmpaType.TIT.Sname = "";
                clsPmpaType.TIT.JinDtl = "";
            }
            else
            {
                clsPmpaType.TIT.Trsno = ArgTRSNO;
                clsPmpaType.TIT.Ipdno = (long)VB.Val(dt.Rows[0]["IPDNO"].ToString().Trim());
                clsPmpaType.TIT.Pano = dt.Rows[0]["Pano"].ToString().Trim();
                clsPmpaType.TIT.GbIpd = dt.Rows[0]["GbIpd"].ToString().Trim();
                clsPmpaType.TIT.InDate = dt.Rows[0]["InDate"].ToString().Trim();
                clsPmpaType.TIT.OutDate = dt.Rows[0]["OutDate"].ToString().Trim();
                clsPmpaType.TIT.ActDate = dt.Rows[0]["ActDate"].ToString().Trim();
                clsPmpaType.TIT.DeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();
                clsPmpaType.TIT.DrCode = dt.Rows[0]["DrCode"].ToString().Trim();
                clsPmpaType.TIT.Ilsu = (int)VB.Val(dt.Rows[0]["Ilsu"].ToString().Trim());
                clsPmpaType.TIT.Bi = dt.Rows[0]["BI"].ToString().Trim();
                clsPmpaType.TIT.Kiho = dt.Rows[0]["KIHO"].ToString().Trim();
                clsPmpaType.TIT.GKiho = dt.Rows[0]["GKIHO"].ToString().Trim();
                clsPmpaType.TIT.PName = dt.Rows[0]["PNAME"].ToString().Trim();
                clsPmpaType.TIT.Gwange = dt.Rows[0]["GWANGE"].ToString().Trim();
                clsPmpaType.TIT.BonRate = (int)VB.Val(dt.Rows[0]["BonRate"].ToString().Trim());
                clsPmpaType.TIT.GISULRATE = (int)VB.Val(dt.Rows[0]["GisulRate"].ToString().Trim());
                clsPmpaType.TIT.GbGameK = dt.Rows[0]["GBGAMEK"].ToString().Trim();
                clsPmpaType.TIT.Bohun = dt.Rows[0]["BOHUN"].ToString().Trim();
                clsPmpaType.TIT.Amset1 = dt.Rows[0]["AMSET1"].ToString().Trim();
                clsPmpaType.TIT.AmSet2 = dt.Rows[0]["AMSET2"].ToString().Trim();
                clsPmpaType.TIT.AmSet3 = dt.Rows[0]["AMSET3"].ToString().Trim();
                clsPmpaType.TIT.AmSet4 = dt.Rows[0]["AMSET4"].ToString().Trim();
                clsPmpaType.TIT.AmSet5 = dt.Rows[0]["AMSET5"].ToString().Trim();
                clsPmpaType.TIT.AmSetB = dt.Rows[0]["AMSETB"].ToString().Trim();
                clsPmpaType.TIT.IllCode1 = dt.Rows[0]["IllCode1"].ToString().Trim();
                clsPmpaType.TIT.IllCode2 = dt.Rows[0]["IllCode2"].ToString().Trim();
                clsPmpaType.TIT.IllCode3 = dt.Rows[0]["IllCode3"].ToString().Trim();
                clsPmpaType.TIT.IllCode4 = dt.Rows[0]["IllCode4"].ToString().Trim();
                clsPmpaType.TIT.IllCode5 = dt.Rows[0]["IllCode5"].ToString().Trim();
                clsPmpaType.TIT.IllCode6 = dt.Rows[0]["IllCode6"].ToString().Trim();
                clsPmpaType.TIT.FromTrans = dt.Rows[0]["FROMTRANS"].ToString().Trim();
                clsPmpaType.TIT.ErAmt = (long)VB.Val(dt.Rows[0]["ErAmt"].ToString().Trim());
                clsPmpaType.TIT.DtGamek = (long)VB.Val(dt.Rows[0]["DTGamek"].ToString().Trim());
                clsPmpaType.TIT.JupboNo = dt.Rows[0]["JUPBONO"].ToString().Trim();
                clsPmpaType.TIT.GbDRG = dt.Rows[0]["GBDRG"].ToString().Trim();
                clsPmpaType.TIT.DrgWRTNO = (long)VB.Val(dt.Rows[0]["DRGWRTNO"].ToString().Trim());
                clsPmpaType.TIT.SangAmt = (long)VB.Val(dt.Rows[0]["SANGAMT"].ToString().Trim());
                clsPmpaType.TIT.OgPdBun = dt.Rows[0]["OGPDBUN"].ToString().Trim();
                clsPmpaType.TIT.OgPdBun2 = dt.Rows[0]["OGPDBUN2"].ToString().Trim();
                clsPmpaType.TIT.OgPdBundtl = dt.Rows[0]["OGPDBUNdtl"].ToString().Trim();
                clsPmpaType.TIT.GelCode = dt.Rows[0]["GelCode"].ToString().Trim();
                clsPmpaType.TIT.MiIlsu = (int)VB.Val(dt.Rows[0]["MiIlsu"].ToString().Trim());
                clsPmpaType.TIT.MiArcDate = dt.Rows[0]["MiArcDate"].ToString().Trim();

                //금액을 Move
                for (i = 1; i <= 60; i++)
                {
                    string strFild = "Amt" + i.ToString("00");
                    clsPmpaType.TIT.Amt[i] = (long)VB.Val(dt.Rows[0][strFild].ToString().Trim()); 
                    if (i <= 20)
                    {
                        clsPmpaType.TIT.TotGub = clsPmpaType.TIT.TotGub + clsPmpaType.TIT.Amt[i];
                    }
                    else if (i <= 49)
                    {
                        clsPmpaType.TIT.TotBigub = clsPmpaType.TIT.TotBigub + clsPmpaType.TIT.Amt[i];
                    }
                }

                for (i = 61; i <= 64; i++)
                {
                    string strFild = "Amt" + i.ToString("00");
                    clsPmpaType.TIT.Amt[i] = (long)VB.Val(dt.Rows[0][strFild].ToString().Trim());
                }
        
                if (clsPmpaType.TIT.Bi == "52" )
		        {
                    clsPmpaType.TIT.Amt[53] = clsPmpaType.TIT.Amt[53];
                }

                clsPmpaType.TIT.Amt[65] = (long)VB.Val(dt.Rows[0]["Amt67"].ToString().Trim());       //2014-02-24
                clsPmpaType.TIT.GbTax = dt.Rows[0]["GbTax"].ToString().Trim();   //2014-02-24
        
                clsPmpaType.TIT.Gbilban2 = dt.Rows[0]["Gbilban2"].ToString().Trim();  //외국new
                clsPmpaType.TIT.GbSpc = dt.Rows[0]["GbSPC"].ToString().Trim();  //선택진료
                clsPmpaType.TIT.Sname = dt.Rows[0]["SName"].ToString().Trim();
                clsPmpaType.TIT.Age = (int)VB.Val(dt.Rows[0]["Age"].ToString().Trim());
                clsPmpaType.TIT.Sex = dt.Rows[0]["Sex"].ToString().Trim();
                clsPmpaType.TIT.TGbSts = dt.Rows[0]["GbSTS"].ToString().Trim();
                clsPmpaType.TIT.WardCode = dt.Rows[0]["WardCode"].ToString().Trim();
                clsPmpaType.TIT.RoomCode = dt.Rows[0]["RoomCode"].ToString().Trim();
                clsPmpaType.TIT.M_InDate = dt.Rows[0]["M_InDate"].ToString().Trim();
                clsPmpaType.TIT.M_OutDate = dt.Rows[0]["M_OutDate"].ToString().Trim();
                clsPmpaType.TIT.M_ActDate = dt.Rows[0]["M_ActDate"].ToString().Trim();
                clsPmpaType.TIT.M_GBSuDay = dt.Rows[0]["GbSuday"].ToString().Trim();  //2013-06-19
                clsPmpaType.TIT.DrgCode = dt.Rows[0]["DrgCode"].ToString().Trim();  //2013-06-25
                clsPmpaType.TIT.M_Ilsu = (int)VB.Val(dt.Rows[0]["M_Ilsu"].ToString().Trim());
                clsPmpaType.TIT.ArcDate = dt.Rows[0]["ArcDate"].ToString().Trim();
                clsPmpaType.TIT.ArcQty = (int)VB.Val(dt.Rows[0]["ArcQty"].ToString().Trim());
                clsPmpaType.TIT.Fee6 = (int)VB.Val(dt.Rows[0]["Fee6"].ToString().Trim());
                clsPmpaType.TIT.GbKekli = dt.Rows[0]["GbKekli"].ToString().Trim();
                clsPmpaType.TIT.IcuQty = (int)VB.Val(dt.Rows[0]["icuqty"].ToString().Trim());
                clsPmpaType.TIT.IcuQty2 = (int)VB.Val(dt.Rows[0]["icuqty2"].ToString().Trim()); //2013-03-15
                clsPmpaType.TIT.GbTewon = dt.Rows[0]["GbTewon"].ToString().Trim(); //퇴원구분
                clsPmpaType.TIT.ROutDate = dt.Rows[0]["ROutDate"].ToString().Trim();
                clsPmpaType.TIT.SimsaTime = dt.Rows[0]["SimsaTime"].ToString().Trim();
                clsPmpaType.TIT.PrintTime = dt.Rows[0]["PrintTime"].ToString().Trim();
                clsPmpaType.TIT.SunapTime = dt.Rows[0]["SunapTime"].ToString().Trim();
                clsPmpaType.TIT.GbCheckList = dt.Rows[0]["GbCheckList"].ToString().Trim();
                clsPmpaType.TIT.MirBuildTime = dt.Rows[0]["MirBuildTime"].ToString().Trim();
                clsPmpaType.TIT.TGbSts = dt.Rows[0]["GBSTS"].ToString().Trim();
                clsPmpaType.TIT.VCode = dt.Rows[0]["VCODE"].ToString().Trim();
                clsPmpaType.TIT.GbSang = dt.Rows[0]["GBSANG"].ToString().Trim();
                clsPmpaType.TIT.MSeqNo = dt.Rows[0]["MSEQNO"].ToString().Trim();
                clsPmpaType.TIT.JinDtl = dt.Rows[0]["JinDtl"].ToString().Trim();  //2012-07-03
                //재원심사--------------------------------------------------------------------
                clsPmpaType.TIT.JSIM_LDATE = dt.Rows[0]["JSIM_LDATE"].ToString().Trim();
                clsPmpaType.TIT.JSIM_SABUN = dt.Rows[0]["JSIM_SABUN"].ToString().Trim();
                clsPmpaType.TIT.JSIM_Set = dt.Rows[0]["JSIM_SET"].ToString().Trim();
                clsPmpaType.TIT.JSIM_OK = dt.Rows[0]["JSIM_OK"].ToString().Trim();
                //DRG정보 --------------------------------------------------------------------
                clsPmpaType.TIT.DRGADC1 = dt.Rows[0]["DRGADC1"].ToString().Trim();
                clsPmpaType.TIT.DRGADC2 = dt.Rows[0]["DRGADC2"].ToString().Trim();
                clsPmpaType.TIT.DRGADC3 = dt.Rows[0]["DRGADC3"].ToString().Trim();
                clsPmpaType.TIT.DRGADC4 = dt.Rows[0]["DRGADC4"].ToString().Trim();
                clsPmpaType.TIT.DRGADC5 = dt.Rows[0]["DRGADC5"].ToString().Trim();
                clsPmpaType.TIT.FCode = dt.Rows[0]["FCODE"].ToString().Trim();
                clsPmpaType.TIT.KTASLEVL = dt.Rows[0]["KTASLEVL"].ToString().Trim(); //2015-12-28
                clsPmpaType.TIT.T_CARE = dt.Rows[0]["T_CARE"].ToString().Trim();  //2015-12-28
                clsPmpaType.TIT.GBHU = dt.Rows[0]["GBHU"].ToString();
            }

            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// clsPmpaType.IMST 변수클리어
        /// 2017-08-21 김민철
        /// </summary>
        /// <seealso cref=""/>
        public void Read_Ipd_Mst_Clear()
        {
            clsPmpaType.IMST.MstCNT = 0; clsPmpaType.IMST.IPDNO = 0; clsPmpaType.IMST.Pano = "";
            clsPmpaType.IMST.Sname = ""; clsPmpaType.IMST.Sex = ""; clsPmpaType.IMST.Age = 0; clsPmpaType.IMST.AgeDays = 0;
            clsPmpaType.IMST.Bi = ""; clsPmpaType.IMST.InDate = ""; clsPmpaType.IMST.InTime = "";
            clsPmpaType.IMST.OutDate = ""; clsPmpaType.IMST.ActDate = ""; clsPmpaType.IMST.Ilsu = 0;
            clsPmpaType.IMST.GbSTS = ""; clsPmpaType.IMST.DeptCode = ""; clsPmpaType.IMST.DrCode = "";
            clsPmpaType.IMST.WardCode = ""; clsPmpaType.IMST.RoomCode = 0; clsPmpaType.IMST.PName = "";
            clsPmpaType.IMST.GbSpc = ""; clsPmpaType.IMST.GbKekli = ""; clsPmpaType.IMST.GbGameK = "";
            clsPmpaType.IMST.GbTewon = ""; clsPmpaType.IMST.Fee6 = 0; clsPmpaType.IMST.Bohun = "";
            clsPmpaType.IMST.Jiyuk = ""; clsPmpaType.IMST.GelCode = ""; clsPmpaType.IMST.Religion = "";
            clsPmpaType.IMST.GbCancer = ""; clsPmpaType.IMST.InOut = ""; clsPmpaType.IMST.Other = "";
            clsPmpaType.IMST.GbDonggi = ""; clsPmpaType.IMST.OgPdBun = ""; clsPmpaType.IMST.OgPdBundtl = ""; clsPmpaType.IMST.Article = "";
            clsPmpaType.IMST.JupboNo = ""; clsPmpaType.IMST.FromTrans = ""; clsPmpaType.IMST.ErAmt = 0;
            clsPmpaType.IMST.ArcDate = ""; clsPmpaType.IMST.ArcQty = 0; clsPmpaType.IMST.GbOldSlip = false;
            clsPmpaType.IMST.IcuQty = 0; clsPmpaType.IMST.Im180 = 0;
            clsPmpaType.IMST.IllCode1 = ""; clsPmpaType.IMST.IllCode2 = ""; clsPmpaType.IMST.IllCode3 = "";
            clsPmpaType.IMST.IllCode4 = ""; clsPmpaType.IMST.IllCode5 = ""; clsPmpaType.IMST.IllCode6 = "";

            clsPmpaType.IMST.TrsDate = ""; clsPmpaType.IMST.Dept1 = ""; clsPmpaType.IMST.Dept2 = "";
            clsPmpaType.IMST.Dept3 = ""; clsPmpaType.IMST.Doctor1 = ""; clsPmpaType.IMST.Doctor2 = "";
            clsPmpaType.IMST.Doctor3 = ""; clsPmpaType.IMST.Ilsu1 = 0; clsPmpaType.IMST.Ilsu2 = 0; clsPmpaType.IMST.Ilsu3 = 0;

            clsPmpaType.IMST.Amset1 = " "; clsPmpaType.IMST.AmSet4 = " "; clsPmpaType.IMST.AmSet5 = "";
            clsPmpaType.IMST.AmSet6 = " "; clsPmpaType.IMST.AmSet7 = " "; clsPmpaType.IMST.AmSet8 = "";
            clsPmpaType.IMST.AmSet9 = ""; clsPmpaType.IMST.AmSetA = ""; clsPmpaType.IMST.MPano = "";
            clsPmpaType.IMST.RDate = ""; clsPmpaType.IMST.TrsCNT = 0; clsPmpaType.IMST.LastTrs = 0;

            clsPmpaType.IMST.IpwonTime = "";
            clsPmpaType.IMST.CancelTime = "";
            clsPmpaType.IMST.GatewonTime = "";
            clsPmpaType.IMST.ROutDate = "";
            clsPmpaType.IMST.SimsaTime = "";
            clsPmpaType.IMST.PrintTime = "";
            clsPmpaType.IMST.SunapTime = "";
            clsPmpaType.IMST.GbCheckList = "";
            clsPmpaType.IMST.MirBuildTime = "";
            clsPmpaType.IMST.Remark = "";
            clsPmpaType.IMST.GBSuDay = "";
            clsPmpaType.IMST.PNEUMONIA = "";
            clsPmpaType.IMST.Pregnant = "";
            clsPmpaType.IMST.GbGoOut = "";
            clsPmpaType.IMST.WardInTime = "";
            clsPmpaType.IMST.TelRemark = "";
            clsPmpaType.IMST.GbExam = "";
            clsPmpaType.IMST.Secret = "";  //2012-11-22
            clsPmpaType.IMST.DrgCode = "";
            clsPmpaType.IMST.KTASLEVL = "";          //2015-12-28
            clsPmpaType.IMST.FROOM = "";
            clsPmpaType.IMST.FROOMETC = "";
            clsPmpaType.IMST.GBJIWON = "";       //2016-07-14
            clsPmpaType.IMST.T_CARE = "";        //2016-07-19            
        }

        /// <summary>
        /// clsPmpaType.TIT 변수클리어
        /// 2017-06-19 김민철
        /// </summary>
        /// <seealso cref="IUMENT.bas(READ_IPD_TRANS_Clear 함수)"/>
        public void Read_Ipd_Trans_Clear()
        {
            int i = 0;
            //clsPmpaType.RPG.Amt1 = new long[51];
            //clsPmpaType.RPG.Amt2 = new long[51];
            //clsPmpaType.RPG.Amt3 = new long[51];
            //clsPmpaType.RPG.Amt4 = new long[51];
            //clsPmpaType.RPG.Amt5 = new long[51];
            //clsPmpaType.RPG.Amt6 = new long[51];
            clsPmpaType.TIT.RAmt = new long[34, 3];
            clsPmpaType.TIT.Amt = new long[96];

            clsPmpaType.TIT.Ipdno = 0; clsPmpaType.TIT.Trsno = 0; clsPmpaType.TIT.Pano = ""; clsPmpaType.TIT.Sname = ""; clsPmpaType.TIT.Jumin3 = "";
            clsPmpaType.TIT.MstCNT = 0;

            clsPmpaType.TIT.GbIpd = ""; clsPmpaType.TIT.InDate = ""; clsPmpaType.TIT.OutDate = ""; clsPmpaType.TIT.ActDate = ""; clsPmpaType.TIT.DeptCode = "";
            clsPmpaType.TIT.DrCode = ""; clsPmpaType.TIT.Ilsu = 0; clsPmpaType.TIT.Bi = ""; clsPmpaType.TIT.Kiho = "";
            clsPmpaType.TIT.Jumin1 = ""; clsPmpaType.TIT.Jumin2 = ""; clsPmpaType.TIT.Jumin3 = "";
            clsPmpaType.TIT.GKiho = ""; clsPmpaType.TIT.PName = ""; clsPmpaType.TIT.Gwange = ""; clsPmpaType.TIT.BonRate = 0; clsPmpaType.TIT.GISULRATE = 0;
            clsPmpaType.TIT.GbGameK = ""; clsPmpaType.TIT.Bohun = "";
            clsPmpaType.TIT.Amset1 = "";
            clsPmpaType.TIT.AmSet2 = "";
            clsPmpaType.TIT.AmSet3 = "";
            clsPmpaType.TIT.AmSet4 = "";
            clsPmpaType.TIT.AmSet5 = "";
            clsPmpaType.TIT.AmSetB = "";
            clsPmpaType.TIT.IllCode1 = "";
            clsPmpaType.TIT.IllCode2 = "";
            clsPmpaType.TIT.IllCode3 = "";
            clsPmpaType.TIT.IllCode4 = "";
            clsPmpaType.TIT.IllCode5 = "";
            clsPmpaType.TIT.IllCode6 = "";

            clsPmpaType.TIT.FromTrans = "";
            clsPmpaType.TIT.ErAmt = 0;
            clsPmpaType.TIT.DtGamek = 0;
            clsPmpaType.TIT.JupboNo = "";
            clsPmpaType.TIT.GbDRG = "";
            clsPmpaType.TIT.DrgWRTNO = 0;
            clsPmpaType.TIT.SangAmt = 0;
            clsPmpaType.TIT.OgPdBun = "";
            clsPmpaType.TIT.OgPdBun2 = "";
            clsPmpaType.TIT.OgPdBundtl = "";
            clsPmpaType.TIT.GelCode = "";
            clsPmpaType.TIT.VCode = "";
            clsPmpaType.TIT.JSIM_REMARK = "";
            clsPmpaType.TIT.JSIM_REMARK9 = "";

            for (i = 0; i < 93; i++)
            {
                clsPmpaType.TIT.Amt[i] = 0;
            }

            clsPmpaType.TIT.Gbilban2 = "";
            clsPmpaType.TIT.GbSpc = "";
            clsPmpaType.TIT.Sname = "";
            clsPmpaType.TIT.Age = 0;
            clsPmpaType.TIT.Sex = "";
            clsPmpaType.TIT.MGbSts = "";
            clsPmpaType.TIT.WardCode = "";
            clsPmpaType.TIT.RoomCode = "";
            clsPmpaType.TIT.M_InDate = "";
            clsPmpaType.TIT.M_OutDate = "";
            clsPmpaType.TIT.M_ActDate = "";
            clsPmpaType.TIT.M_Ilsu = 0;
            clsPmpaType.TIT.MiIlsu = 0;
            clsPmpaType.TIT.MiArcDate = "";
            clsPmpaType.TIT.M_GBSuDay = "";
            clsPmpaType.TIT.DrgCode = "";
            clsPmpaType.TIT.ArcDate = "";
            clsPmpaType.TIT.ArcQty = 0;
            clsPmpaType.TIT.Fee6 = 0;
            clsPmpaType.TIT.GbKekli = "";
            clsPmpaType.TIT.IcuQty = 0;
            clsPmpaType.TIT.GbTewon = "";
            clsPmpaType.TIT.ROutDate = "";
            clsPmpaType.TIT.SimsaTime = "";
            clsPmpaType.TIT.PrintTime = "";
            clsPmpaType.TIT.SunapTime = "";
            clsPmpaType.TIT.GbCheckList = "";
            clsPmpaType.TIT.MirBuildTime = "";
            clsPmpaType.TIT.TGbSts = "";
            clsPmpaType.TIT.VCode = "";
            clsPmpaType.TIT.GbSang = "";
            clsPmpaType.TIT.JSIM_LDATE = "";
            clsPmpaType.TIT.JSIM_SABUN = "";
            clsPmpaType.TIT.JSIM_Set = "";
            clsPmpaType.TIT.JSIM_OK = "";
            clsPmpaType.TIT.FCode = "";
            clsPmpaType.TIT.KTASLEVL = "";
            clsPmpaType.TIT.T_CARE = "";
            clsPmpaType.TIT.GBJIWON = "";
            clsPmpaType.TIT.TROWID = "";
            clsPmpaType.TIT.GBHU = "";
        }

        /// <summary>
        ///  제원중인 환자인지 체크
        /// </summary>
        /// <param name="nIPDNO"></param>
        /// <returns></returns>
        public bool Ipd_roomChaAmt_Info(PsmhDb pDbCon, string ArgRoom)
        {
            bool rtnVal = false;

            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT * FROM " + ComNum.DB_PMPA + "BAS_ROOM ";
            SQL += ComNum.VBLF + "  WHERE roomcode='" + ArgRoom + "' ";
            SQL += ComNum.VBLF + "    AND transdate1 <= trunc(sysdate) ";
            SQL += ComNum.VBLF + "    AND overamt > 0 ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = false;
                return rtnVal;
            }
            if (Dt.Rows.Count > 0)
            {
                rtnVal = true;
            }

            Dt.Dispose();
            Dt = null;

            return rtnVal;
        }

        /// <summary>
        /// IPD_TRANS을 읽어 금액을 SSAmt Sheet에 표시함
        /// ex: Display_Ipd_Trans_Amt(SSAmt,0,12324) -> IPD_TRANS TRSNO=12324의 금액을 표시
        ///     Display_Ipd_Trans_Amt(SSAmt,12345,0) -> IPD_TRANS IPDNO=12345 합계 금액을 표시
        ///     IPD_NEW_SLIP 금액 재계산 않함.
        /// <param name="SSAmt">Spread Name</param>
        /// <param name="nIPDNO">입원번호</param>
        /// <param name="nTRSNO">자격번호</param>
        /// 2017-07-10 김민철
        /// </summary>
        /// <seealso cref="IUMENT.bas(IPD_TRANS_Amt_Display 함수)"/>
        public void Display_Ipd_Trans_Amt(PsmhDb pDbCon, FpSpread SSAmt, long nIPDNO, long nTRSNO)
        {
            long[] nAmt = new long[61];
            long nIpdno1 = 0, nTot1 = 0, nTot2 = 0;
            int i = 0, j = 0, nREAD = 0;
            string strGbSTS = "";
            clsIpdAcct cIAcct = new clsIpdAcct();

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = "";

            clsDB.setBeginTran(pDbCon);

            #region 조합부담, 본인부담, 할인금액 계산함
            if (nTRSNO > 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT GBSTS, AMT53 FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                SQL += ComNum.VBLF + "  WHERE TRSNO = " + nTRSNO + " ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                //if (Dt.Rows[0]["GBSTS"].ToString().Trim() == "0" || Dt.Rows[0]["GBSTS"].ToString().Trim() == "3" || Dt.Rows[0]["GBSTS"].ToString().Trim() == "7" || Convert.ToInt64(Dt.Rows[0]["AMT53"].ToString()) == 0)  //오류 2018-10-10 kyo
                if (Dt.Rows[0]["GBSTS"].ToString().Trim() == "0" || Dt.Rows[0]["GBSTS"].ToString().Trim() == "3" || (Dt.Rows[0]["GBSTS"].ToString().Trim() == "7" && Convert.ToInt64(Dt.Rows[0]["AMT53"].ToString()) == 0))
                {
                    //조합부담, 본인부담, 할인금액을 계산함
                    if (cIAcct.Ipd_Tewon_Amt_Process(pDbCon, nTRSNO, "", "") == false)
                    {
                        Dt.Dispose();
                        Dt = null;
                        ComFunc.MsgBox("조합부담,본인부담,할인액을 계산 도중에 오류가 발생함!!", "확인");
                        clsDB.setRollbackTran(pDbCon);
                        return;
                    }
                }
                Dt.Dispose();
                Dt = null;
            }
            #endregion

            clsDB.setCommitTran(pDbCon);

            #region 누적별 금액 합산
            SQL = "";
            SQL += ComNum.VBLF + " SELECT Bi,TO_CHAR(InDate,'YYYY-MM-DD') InDate, IPDNO, GBSTS, ";
            SQL += ComNum.VBLF + "        Amt01,Amt02,Amt03,Amt04,Amt05,Amt06,Amt07,Amt08,Amt09,Amt10,";
            SQL += ComNum.VBLF + "        Amt11,Amt12,Amt13,Amt14,Amt15,Amt16,Amt17,Amt18,Amt19,Amt20,";
            SQL += ComNum.VBLF + "        Amt21,Amt22,Amt23,Amt24,Amt25,Amt26,Amt27,Amt28,Amt29,Amt30,";
            SQL += ComNum.VBLF + "        Amt31,Amt32,Amt33,Amt34,Amt35,Amt36,Amt37,Amt38,Amt39,Amt40,";
            SQL += ComNum.VBLF + "        Amt41,Amt42,Amt43,Amt44,Amt45,Amt46,Amt47,Amt48,Amt49,Amt50,";
            SQL += ComNum.VBLF + "        Amt51,Amt52,Amt53,Amt54,Amt55,Amt56,Amt57,Amt58,Amt59,Amt60,Amt64 ";
            SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
            if (nTRSNO == 0)
            {
                SQL += ComNum.VBLF + "WHERE IPDNO = " + nIPDNO + " ";
            }
            else
            {
                SQL += ComNum.VBLF + "WHERE TrsNo = " + nTRSNO + " ";
            }
            SQL += ComNum.VBLF + "ORDER BY InDate,Bi ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            nREAD = Dt.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                for (j = 1; j < 61; j++)
                {
                    nAmt[j] = nAmt[j] + Convert.ToInt32(Dt.Rows[i]["AMT" + VB.Format(j, "00")].ToString());
                }
            }
            if (nREAD > 0)
            {
                nIpdno1 = Convert.ToInt32(Dt.Rows[0]["IPDNO"].ToString());
                strGbSTS = Dt.Rows[0]["GBSTS"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            #endregion

            #region 보증금,중간납 입금액
            nAmt[51] = 0; nAmt[52] = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SuNext,SUM(Amt) Amt FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH ";
            SQL += ComNum.VBLF + "  WHERE IPDNO = " + nIpdno1 + " ";
            SQL += ComNum.VBLF + "    AND SuNext IN ('Y88') ";
            SQL += ComNum.VBLF + "  GROUP BY SuNext ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (Dt.Rows.Count > 0)
            {
                nAmt[51] = Convert.ToInt32(Dt.Rows[0]["Amt"].ToString());
            }

            Dt.Dispose();
            Dt = null;

            if (nAmt[51] == 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SuNext,SUM(Amt) Amt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH ";
                SQL += ComNum.VBLF + "  WHERE IPDNO=" + nIpdno1 + " ";
                SQL += ComNum.VBLF + "    AND SuNext IN ('Y85','Y87') ";
                SQL += ComNum.VBLF + "  GROUP BY SuNext ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    nAmt[52] += Convert.ToInt32(Dt.Rows[i]["Amt"].ToString());
                }

                Dt.Dispose();
                Dt = null;
            }
            #endregion

            #region ( 급여, 비급여 합계금액을 계산 )
            nTot1 = 0;
            nTot2 = 0;
            for (i = 1; i < 50; i++)
            {
                if (i >= 1 && i <= 20)
                {
                    nTot1 += nAmt[i];
                }
                else
                {
                    nTot2 += nAmt[i];
                }
            }
            #endregion

            #region 금액 DISPLAY
            for (i = 1; i < 59; i++)
            {
                if (i >= 11 && i <= 20)
                {
                    SSAmt.Sheets[0].Cells[i - 11, 3].Text = VB.Format(nAmt[i], "###,###,##0") + " ";
                }
                else if (i >= 21 && i <= 30)
                {
                    SSAmt.Sheets[0].Cells[i - 21, 5].Text = VB.Format(nAmt[i], "###,###,##0") + " ";
                }
                else if (i >= 31 && i <= 40)
                {
                    SSAmt.Sheets[0].Cells[i - 31, 7].Text = VB.Format(nAmt[i], "###,###,##0") + " ";
                }
                else if (i >= 41 && i <= 49)
                {
                    SSAmt.Sheets[0].Cells[i - 41, 9].Text = VB.Format(nAmt[i], "###,###,##0") + " ";
                }
                else if (i == 50)
                {
                    SSAmt.Sheets[0].Cells[10, 9].Text = VB.Format(nAmt[i], "###,###,##0") + " ";
                }
                else if (i >= 51 && i <= 55)
                {
                    if (i == 51)
                    {
                        if (nAmt[51] != 0)
                        {
                            SSAmt.Sheets[0].Cells[12, (i - 51) * 2 + 1].Text = VB.Format(nAmt[i], "###,###,##0") + " ";
                        }
                        else
                        {
                            SSAmt.Sheets[0].Cells[12, (i - 51) * 2 + 1].Text = "0";
                        }
                    }
                    else if (i == 52)
                    {
                        if (nAmt[52] != 0)
                        {
                            SSAmt.Sheets[0].Cells[12, (i - 51) * 2 + 1].Text = VB.Format(nAmt[i], "###,###,##0") + " ";
                        }
                        else
                        {
                            SSAmt.Sheets[0].Cells[12, (i - 51) * 2 + 1].Text = "0";
                        }
                    }
                    else if (i == 55)
                    {
                        if (nAmt[51] != 0)
                        {
                            SSAmt.Sheets[0].Cells[12, (i - 51) * 2 + 1].Text = VB.Format(nAmt[i] - nAmt[56], "###,###,##0") + " ";
                        }
                        else
                        {
                            SSAmt.Sheets[0].Cells[12, (i - 51) * 2 + 1].Text = VB.Format(nAmt[51] - nAmt[52], "###,###,##0") + " ";
                        }
                    }
                    else
                    {
                        SSAmt.Sheets[0].Cells[12, (i - 51) * 2 + 1].Text = VB.Format(nAmt[i], "###,###,##0") + " ";
                    }
                }
                else if (i >= 56 && i <= 58)
                {
                    SSAmt.Sheets[0].Cells[13, (i - 54) * 2 + 1].Text = VB.Format(nAmt[i], "###,###,##0") + " ";
                }
                else
                {
                    SSAmt.Sheets[0].Cells[i - 1, 1].Text = VB.Format(nAmt[i], "###,###,##0") + " ";
                }
            }

            SSAmt.Sheets[0].Cells[10, 3].Text = VB.Format(nTot1, "###,###,##0") + " ";
            SSAmt.Sheets[0].Cells[10, 7].Text = VB.Format(nTot2, "###,###,##0") + " ";

            #endregion

        }

        /// <summary>
        /// IPD_TRANS을 읽어 금액을 SSAmt Sheet에 표시함
        /// <param name="SSAmt">Spread Name</param>
        /// <param name="nIPDNO">입원번호</param>
        /// <param name="nTRSNO">자격번호</param>
        /// <param name="strTempTrans"></param>
        /// <param name="strPano">등록번호</param>
        /// 2017-11-21 박창욱
        /// </summary>
        /// <seealso cref="IUMENT.bas(IPD_TRANS_Amt_Display_NEW2 함수)"/>
        public void DISPLAY_IPD_TRANS_AMT_NEW(PsmhDb pDbCon, FpSpread SSAmt, FpSpread SSAmtNew, FpSpread ssDrg, FpSpread ssDrgAmt, long nIPDNO, long nTRSNO, string strTempTrans, string strPano, string strDrg, [Optional] string strHUbyAct)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;
            double[] nAmt = new double[61];
            double nIpdno1 = 0;
            double nAmtSang = 0;
            string strGbSTS = "";
            string strBi = "";
            string strOBPDBun = "";
            string strOBPDBundtl = "";
            string strVCode = "";
            string strInDate = "";
            string strOutDate_Drg = string.Empty;
            string strNgt = string.Empty;
            string strDrgCode = string.Empty;

            clsIpdAcct cIAcct = new clsIpdAcct();
            DRG DRG = new DRG();

            ComFunc.ReadSysDate(pDbCon);

            //누적할 배열 변수를 Clear
            for (i = 0; i < 61; i++)
            {
                nAmt[i] = 0;
            }
            nAmtSang = 0;

            //자격정보 읽기
            Read_Ipd_Mst_Trans(pDbCon, strPano, nTRSNO, strTempTrans);

            strDrgCode = clsPmpaType.TIT.DrgCode;

            if (strHUbyAct == "1")
            {
                if (clsPmpaType.TIT.GBHU != "Y" || clsPmpaType.TIT.GBHU == "")
                {
                    ComFunc.MsgBox("호스피스 행위별 수가 조회는 호스피스로 심사가 완료된 내역만 가능합니다.");
                    return;
                }
            }

            if (clsPmpaType.TIT.InDate == clsPmpaType.TIT.OutDate  && Ipd_roomChaAmt_Info(pDbCon, clsPmpaType.TIT.RoomCode) == true)

            {
                ComFunc.MsgBox("당일퇴원 상급병실사용정보가 있습니다.확인바랍니다.");
            }
            try
            {
                #region 재원금액 계산 (심사완료 전까지)
                if (nTRSNO != 0)
                {
                    SQL = "";
                    SQL = " SELECT GBSTS, AMT53 FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                    SQL = SQL + ComNum.VBLF + " WHERE TRSNO = " + nTRSNO + " ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    //재원자, 대조리스트만 작업함
                    if ((dt.Rows[0]["GBSTS"].ToString().Trim() == "0" || dt.Rows[0]["GBSTS"].ToString().Trim() == "3" ||
                        dt.Rows[0]["GBSTS"].ToString().Trim() == "2" || dt.Rows[0]["GBSTS"].ToString().Trim() == "1") && strHUbyAct != "1" )
                    {
                        clsDB.setBeginTran(pDbCon);

                        //IPD_TRANS AMT01-AMT50 다시 계산함.
                        if (cIAcct.Ipd_Trans_Amt_ReBuild(pDbCon, nTRSNO, strTempTrans) == false)
                        {
                            Cursor.Current = Cursors.Default;
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox("총진료비를 재계산 도중에 오류가 발생함!!, 전산실로 연락바람!!!");
                            return;
                        }

                        //DRG 환자구분
                        if (strDrg == "D" && strDrgCode != "")
                        {
                            //퇴원일자가 없으면 현재일자로 세팅
                            if (clsPmpaType.TIT.OutDate == "")
                            {
                                strOutDate_Drg = clsPublic.GstrSysDate;
                            }
                            else
                            {
                                strOutDate_Drg = clsPmpaType.TIT.OutDate;
                            }

                            strInDate = clsPmpaType.TIT.InDate;
                            //strOutDate_Drg = "2017-06-30";

                            strNgt = DRG.Read_GbNgt_DRG(pDbCon, strPano, nIPDNO, nTRSNO);

                            if (DRG.READ_DRG_AMT_MASTER(pDbCon, strDrgCode, strPano, nIPDNO, nTRSNO, strNgt, strInDate, strOutDate_Drg) == false)
                            {
                                Cursor.Current = Cursors.Default;
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox("DRG 금액을 계산 도중에 오류가 발생함!!");
                                return;
                            }
                        }
                        else
                        {   //일반 환자인 경우
                            //조합부담, 본인부담, 할인금액을 계산
                            if (cIAcct.Ipd_Tewon_Amt_Process(pDbCon, nTRSNO, strTempTrans, "") == false)
                            {
                                Cursor.Current = Cursors.Default;
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox("조합부담,본인부담,할인액을 계산 도중에 오류가 발생함!!");
                                return;
                            }
                        }

                        clsDB.setCommitTran(pDbCon);

                    }

                    dt.Dispose();
                    dt = null;

                }
                #endregion

                #region //누적별 금액을 합산함 nAmt[1] ~ nAmt[60];
                SQL = "";
                SQL += ComNum.VBLF + " SELECT Bi,TO_CHAR(InDate,'YYYY-MM-DD') InDate, IPDNO, GBSTS, OGPDBUN,OGPDBUNdtl,VCode";
                SQL += ComNum.VBLF + "        ,Amt01,Amt02,Amt03,Amt04,Amt05,Amt06,Amt07,Amt08,Amt09,Amt10                  ";
                SQL += ComNum.VBLF + "        ,Amt11,Amt12,Amt13,Amt14,Amt15,Amt16,Amt17,Amt18,Amt19,Amt20                  ";
                SQL += ComNum.VBLF + "        ,Amt21,Amt22,Amt23,Amt24,Amt25,Amt26,Amt27,Amt28,Amt29,Amt30                  ";
                SQL += ComNum.VBLF + "        ,Amt31,Amt32,Amt33,Amt34,Amt35,Amt36,Amt37,Amt38,Amt39,Amt40                  ";
                SQL += ComNum.VBLF + "        ,Amt41,Amt42,Amt43,Amt44,Amt45,Amt46,Amt47,Amt48,Amt49,Amt50                  ";
                SQL += ComNum.VBLF + "        ,Amt51,Amt52,Amt53,Amt54,Amt55,Amt56,Amt57,Amt58,Amt59,Amt60                  ";
                SQL += ComNum.VBLF + "        ,AMT61,AMT62,AMT63,AMT64,SANGAMT                                              ";

                if (strTempTrans == "임시자격") //2012-09-07
                {
                    SQL += ComNum.VBLF + "        ,0 AMT67    ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "WORK_IPD_TRANS_TERM ";
                    if (nTRSNO == 0)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE IPDNO = " + nIPDNO + " ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE TrsNo = " + nTRSNO + " ";
                    }
                    SQL = SQL + ComNum.VBLF + " AND ROWID = '" + clsPmpaType.TIT.TROWID + "' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "        ,AMT67    ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                    if (nTRSNO == 0)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE IPDNO = " + nIPDNO + " ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE TrsNo = " + nTRSNO + " ";
                    }
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY InDate,Bi ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    for (k = 1; k < 61; k++)
                    {
                        nAmt[k] += VB.Val(dt.Rows[0]["Amt" + k.ToString("00")].ToString().Trim());
                    }

                    nIpdno1 = VB.Val(dt.Rows[0]["IPDNO"].ToString().Trim());
                    strGbSTS = dt.Rows[0]["GBSTS"].ToString().Trim();
                    nAmtSang = VB.Val(dt.Rows[0]["SANGAMT"].ToString().Trim());
                    strBi = dt.Rows[0]["Bi"].ToString().Trim();
                    clsPmpaType.TIT.MGbSts = strGbSTS;
                    clsPmpaType.TIT.Amt[61] = (long)VB.Val(dt.Rows[0]["Amt61"].ToString().Trim());
                    clsPmpaType.TIT.Amt[62] = (long)VB.Val(dt.Rows[0]["Amt62"].ToString().Trim());
                    clsPmpaType.TIT.Amt[63] = (long)VB.Val(dt.Rows[0]["Amt63"].ToString().Trim());
                    clsPmpaType.TIT.Amt[64] = (long)VB.Val(dt.Rows[0]["Amt64"].ToString().Trim());  //약제상한차액
                    clsPmpaType.TIT.Amt[65] = (long)VB.Val(dt.Rows[0]["Amt67"].ToString().Trim());  //부가세

                    strOBPDBun = dt.Rows[0]["OGPDBUN"].ToString().Trim();
                    strOBPDBundtl = dt.Rows[0]["OGPDBUNdtl"].ToString().Trim();

                    strInDate = dt.Rows[0]["InDate"].ToString().Trim();
                    strVCode = dt.Rows[0]["VCode"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                #endregion

                // 영수증 항목별금액 합산 vb60new\report_print2.bas
                if (strDrg == "D")
                {
                    #region DRG 이전 Data를 위한 루틴
                    //고도화 사업 이전 DRG 계산을 위한 임시용 루틴입니다.
                    //일정시점이 지나면 아래루틴을 제거 하세요.
                    if (clsPmpaType.TIT.Amt[70] >= 0 )           //DRG 예전버전인 경우 
                    //if ( 1 == 2)           //DRG 예전버전인 경우 
                        {
                        //퇴원일자가 없으면 현재일자로 세팅
                        if (clsPmpaType.TIT.OutDate == "")
                        {
                            strOutDate_Drg = clsPublic.GstrSysDate;
                        }
                        else
                        {
                            strOutDate_Drg = clsPmpaType.TIT.OutDate;
                        }

                        strInDate = clsPmpaType.TIT.InDate;
                        //strOutDate_Drg = "2017-06-30";

                        strNgt = DRG.Read_GbNgt_DRG(pDbCon, strPano, nIPDNO, nTRSNO);

                        if (DRG.READ_DRG_AMT_MASTER(pDbCon, strDrgCode, strPano, nIPDNO, nTRSNO, strNgt, strInDate, strOutDate_Drg) == false)
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("DRG 금액을 계산 도중에 오류가 발생함!!");
                            return;
                        }
                    }
                    #endregion

                    //DRG 금액 RPG.Amt 변수에 저장 (인정비급여, 선별급여 포함)
                    Ipd_Trans_PrtAmt_Read_Drg(pDbCon, nTRSNO);

                    if (ssDrg != null && ssDrgAmt != null)
                    {
                        //DRG 세부내역 표시
                        Ipd_Tewon_PrtAmt_Gesan_Drg(pDbCon, ssDrg, ssDrgAmt, strPano, nIPDNO, nTRSNO);
                    }
                    else
                    {   //DRG 식대본인부담 계산
                        clsPmpaType.RPG.Amt5[3] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[3] * clsPmpaType.IBR.Food / 100.0);
                        clsPmpaType.RPG.Amt6[3] = clsPmpaType.RPG.Amt1[3] - clsPmpaType.RPG.Amt5[3];
                        clsPmpaType.RPG.Amt5[50] += clsPmpaType.RPG.Amt5[3];
                        clsPmpaType.RPG.Amt6[50] += clsPmpaType.RPG.Amt6[3];
                    }
                }
                else
                {
                    
                    if (strHUbyAct == "1")
                    {
                        Ipd_Trans_PrtAmt_Read(pDbCon, nTRSNO, strTempTrans, "1");
                        Ipd_Tewon_PrtAmt_Gesan_HU(pDbCon, strPano, nIPDNO, nTRSNO, strTempTrans, "", "1");
                    }
                    else
                    {
                        Ipd_Trans_PrtAmt_Read(pDbCon, nTRSNO, strTempTrans);
                        Ipd_Tewon_PrtAmt_Gesan(pDbCon, strPano, nIPDNO, nTRSNO, strTempTrans, "");
                    }
                    
                }

                // 영수증 발행용 금액을 계산함
                Report_AmtSet_RAmt(nAmt, strBi, strDrg);

                #region // 치료재료대 금액을 계산함
                SQL = "";
                if (strHUbyAct == "1")
                {
                    SQL = "SELECT Nu,SUM(Amt1 * -1) Amt ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1 ";
                    SQL = SQL + ComNum.VBLF + "   AND TRIM(PART) = '!-'";
                }
                else
                {
                    SQL = "SELECT Nu,SUM(Amt1) Amt ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1 ";
                }
                

                if (nTRSNO == 0)
                {
                    SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + nIPDNO + " ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND TrsNo = " + nTRSNO + " ";
                }

                if (strTempTrans == "임시자격")
                {
                    SQL = SQL + ComNum.VBLF + "  AND BDate>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND BDate<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD') ";
                }

                SQL = SQL + ComNum.VBLF + " AND Bun IN ('29','31','32','33','36','39') ";
                SQL = SQL + ComNum.VBLF + "GROUP BY Nu ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    //급여 처치재료대
                    if (string.Compare(dt.Rows[i]["Nu"].ToString().Trim(), "20") <= 0)
                    {
                        clsPmpaType.TIT.RAmt[10, 1] += (long)VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                        clsPmpaType.TIT.RAmt[7, 1] -= (long)VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    }
                    else
                    {
                        clsPmpaType.TIT.RAmt[10, 2] += (long)VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                        clsPmpaType.TIT.RAmt[7, 2] -= (long)VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    }
                }
                dt.Dispose();
                dt = null;
                #endregion

                #region // 계산된 진료비 DISPLAY
                nAmt[51] = 0;
                nAmt[52] = 0;

                SQL = "";
                SQL = "SELECT SUM(SUM(CASE WHEN SUNEXT = 'Y88' THEN AMT END)) Y88 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH  ";
                SQL = SQL + ComNum.VBLF + " WHERE TRSNO =" + nTRSNO + " ";
                SQL = SQL + ComNum.VBLF + "   AND SuNext IN ('Y88') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY SuNext ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nAmt[51] = VB.Val(dt.Rows[0]["Y88"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                if (nAmt[51] == 0)
                {
                    SQL = "";
                    SQL = "SELECT SUM(SUM(CASE WHEN SUNEXT = 'Y88' THEN AMT END)) Y88, ";
                    SQL = SQL + ComNum.VBLF + " SUM(SUM(CASE WHEN SUNEXT IN ('Y85','Y87') THEN AMT END )) Y8785 ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH  ";
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO=" + nIpdno1 + " ";
                    SQL = SQL + ComNum.VBLF + "   AND SuNext IN ('Y88','Y85','Y87') ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY SuNext ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        nAmt[52] = VB.Val(dt.Rows[0]["Y8785"].ToString().Trim()) - VB.Val(dt.Rows[0]["Y88"].ToString().Trim());
                    }

                    dt.Dispose();
                    dt = null;
                }

                SSAmt_Display(SSAmt, nAmt, nAmtSang, strOBPDBun, strInDate, strVCode, strDrg);          //(구)영수증약식
                SSAmt_Display_New(SSAmtNew, nAmt, nAmtSang, strOBPDBun, strInDate, strVCode, strDrg);   //(신)영수증양식

                #endregion
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 영수증 표시용 변수 세팅
        /// </summary>
        /// <param name="nAmt"></param>
        /// <param name="strBi"></param>
        /// <param name="strDrg"></param>
        void Report_AmtSet_RAmt(double[] nAmt, string strBi, string strDrg)
        {
            int i = 0;

            for (i = 1; i < 24; i++)
            {
                clsPmpaType.TIT.RAmt[i, 1] = 0;
                clsPmpaType.TIT.RAmt[i, 2] = 0;
            }

            clsPmpaType.TIT.RAmt[33, 2] = 0;

            //1.진찰료
            clsPmpaType.TIT.RAmt[1, 1] = (long)nAmt[1];

            //2.입원료
            clsPmpaType.TIT.RAmt[2, 1] = (long)(nAmt[2] + nAmt[3]);
            clsPmpaType.TIT.RAmt[2, 2] = (long)nAmt[21];

            //3.식대
            clsPmpaType.TIT.RAmt[3, 1] = (long)nAmt[16];
            clsPmpaType.TIT.RAmt[3, 2] = (long)nAmt[34];

            //4.투약 및 조제료
            clsPmpaType.TIT.RAmt[4, 1] = (long)(nAmt[4] - clsPmpaType.TIT.Amt[64]);
            clsPmpaType.TIT.RAmt[4, 2] = (long)nAmt[22];

            //5.주사료
            clsPmpaType.TIT.RAmt[5, 1] = (long)nAmt[5];
            clsPmpaType.TIT.RAmt[5, 2] = (long)nAmt[23];

            //6.마취료
            clsPmpaType.TIT.RAmt[6, 1] = (long)nAmt[6];
            clsPmpaType.TIT.RAmt[6, 2] = (long)nAmt[24];

            //7.처치 및 수술료
            clsPmpaType.TIT.RAmt[7, 1] = (long)(nAmt[9] + nAmt[10] + nAmt[12]);
            clsPmpaType.TIT.RAmt[7, 2] = (long)(nAmt[27] + nAmt[28] + nAmt[30]);

            //8.검사료
            clsPmpaType.TIT.RAmt[8, 1] = (long)(nAmt[13] + nAmt[14]);
            clsPmpaType.TIT.RAmt[8, 2] = (long)(nAmt[31] + nAmt[32]);

            //9.방사선료
            clsPmpaType.TIT.RAmt[9, 1] = (long)nAmt[15];
            clsPmpaType.TIT.RAmt[9, 2] = (long)nAmt[33];

            //10.치료재료대(아래에서 별도 계산)
            clsPmpaType.TIT.RAmt[10, 1] = 0;
            clsPmpaType.TIT.RAmt[10, 2] = 0;

            //11.전액본인부담
            clsPmpaType.TIT.RAmt[11, 1] = 0;
            clsPmpaType.TIT.RAmt[11, 2] = 0;

            //12.재활및물리치료
            clsPmpaType.TIT.RAmt[12, 1] = (long)nAmt[7];
            clsPmpaType.TIT.RAmt[12, 2] = (long)nAmt[25];

            //13.정신요법료
            clsPmpaType.TIT.RAmt[13, 1] = (long)nAmt[8];
            clsPmpaType.TIT.RAmt[13, 2] = (long)nAmt[26];

            //18.수혈료
            clsPmpaType.TIT.RAmt[18, 1] = (long)nAmt[11];
            clsPmpaType.TIT.RAmt[18, 2] = (long)nAmt[29];

            //14.CT
            clsPmpaType.TIT.RAmt[14, 1] = (long)nAmt[19];
            clsPmpaType.TIT.RAmt[14, 2] = (long)nAmt[37];

            //15.MRI
            clsPmpaType.TIT.RAmt[15, 1] = (long)nAmt[18];
            clsPmpaType.TIT.RAmt[15, 2] = (long)nAmt[38];

            //TA환자는 초음파 보험회사에 청구
            if (strBi == "52")
            {
                //16. 초음파
                clsPmpaType.TIT.RAmt[16, 1] = (long)nAmt[36];
                clsPmpaType.TIT.RAmt[16, 2] = 0;

                //17.보철, 교정료
                clsPmpaType.TIT.RAmt[17, 1] = 0;
                clsPmpaType.TIT.RAmt[17, 2] = (long)nAmt[40];
            }
            else
            {
                //16. 초음파
                clsPmpaType.TIT.RAmt[16, 1] = 0;
                clsPmpaType.TIT.RAmt[16, 2] = (long)nAmt[36];

                //17.보철, 교정료
                clsPmpaType.TIT.RAmt[17, 1] = 0;
                clsPmpaType.TIT.RAmt[17, 2] = (long)nAmt[40];
            }

            //19.예약진찰료
            clsPmpaType.TIT.RAmt[19, 1] = 0;
            clsPmpaType.TIT.RAmt[19, 2] = 0;

            //20.증명료
            clsPmpaType.TIT.RAmt[20, 1] = 0;
            clsPmpaType.TIT.RAmt[20, 2] = (long)nAmt[47];

            //21.병실차액
            clsPmpaType.TIT.RAmt[21, 1] = 0;
            clsPmpaType.TIT.RAmt[21, 2] = (long)nAmt[35];

            //22.기타
            clsPmpaType.TIT.RAmt[22, 1] = (long)(nAmt[17] + nAmt[20]);
            clsPmpaType.TIT.RAmt[22, 2] = (long)(nAmt[39] + nAmt[41] + nAmt[42] + nAmt[43]);
            clsPmpaType.TIT.RAmt[22, 2] += (long)(nAmt[45] + nAmt[46] + nAmt[48]);

            //선택진료비
            clsPmpaType.TIT.RAmt[33, 2] = (long)nAmt[44];

            //23.합계
            clsPmpaType.TIT.RAmt[23, 1] = 0;
            clsPmpaType.TIT.RAmt[23, 2] = 0;
            for (i = 1; i < 23; i++)
            {
                clsPmpaType.TIT.RAmt[23, 1] += clsPmpaType.TIT.RAmt[i, 1];
                clsPmpaType.TIT.RAmt[23, 2] += clsPmpaType.TIT.RAmt[i, 2];
            }
            //선택진료비 포함
            clsPmpaType.TIT.RAmt[23, 2] += clsPmpaType.TIT.RAmt[33, 2];

            //24.급여 본인부담액(비급여합계를 뺀금액) =
            //NAMT(53)의 금액은 비급여금액이 포함된금액입니다.
            if (strBi == "52")
            {
                nAmt[53] = nAmt[53];
                clsPmpaType.TIT.RAmt[24, 1] = clsPmpaType.TIT.RAmt[23, 1];
            }
            else
            {
                clsPmpaType.TIT.RAmt[24, 1] = clsPmpaType.TIT.RAmt[23, 1] - (long)nAmt[53];
            }

            //25.급여 보험자부담액
            clsPmpaType.TIT.RAmt[25, 1] = (long)nAmt[53];

            //27.진료비총액
            clsPmpaType.TIT.RAmt[26, 1] = clsPmpaType.TIT.RAmt[23, 1] + clsPmpaType.TIT.RAmt[23, 2];

            //28.환자부담총액 = 급여본인부담 + 비급여합계
            clsPmpaType.TIT.RAmt[27, 1] = clsPmpaType.TIT.RAmt[24, 1] + clsPmpaType.TIT.RAmt[23, 2];

            //29.이미 납부한 금액
            clsPmpaType.TIT.RAmt[28, 1] = (long)nAmt[51];

            //30.할인액
            clsPmpaType.TIT.RAmt[29, 1] = (long)nAmt[54];

            //31.미수액
            clsPmpaType.TIT.RAmt[30, 1] = (long)nAmt[56];

            //32.수납금액
            clsPmpaType.TIT.RAmt[31, 1] = (long)nAmt[57];
            if (nAmt[58] != 0)
            {
                clsPmpaType.TIT.RAmt[31, 1] = (long)nAmt[58] * -1;
            }

            //DRG환자는 총금액, 조합, 본인부담 다시 설정함
            if (strDrg == "D")
            {
                //25.급여 보험자부담액
                clsPmpaType.TIT.RAmt[25, 1] = (long)nAmt[53];
                //27.진료비총액
                clsPmpaType.TIT.RAmt[26, 1] = (long)nAmt[50];
                //28.환자부담총액 = 급여본인부담 + 비급여합계
                clsPmpaType.TIT.RAmt[27, 1] = (long)nAmt[55];
            }

        }

        /// <summary>
        /// Description : 입원진료비 금액 (구)화면표시
        /// Author : 김민철
        /// Create Date : 2018.02.01
        /// </summary>
        /// <param name="SSAmt"></param>
        /// <param name="nAmt"></param>
        /// <param name="nAmtSang"></param>
        /// <param name="strOBPDBun"></param>
        /// <param name="strInDate"></param>
        /// <param name="strVCode"></param>
        void SSAmt_Display(FpSpread SSAmt, double[] nAmt, double nAmtSang, string strOBPDBun, string strInDate, string strVCode, string strDrg)
        {
            #region 항목별 금액표시
            //진찰
            SSAmt.ActiveSheet.Cells[0, 2].Text = clsPmpaType.RPG.Amt5[1].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[0, 3].Text = clsPmpaType.RPG.Amt6[1].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[0, 4].Text = clsPmpaType.RPG.Amt4[1].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[0, 5].Text = clsPmpaType.RPG.Amt3[1].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[0, 6].Text = clsPmpaType.RPG.Amt2[1].ToString("#,##0 ");    //비급여

            //입원
            SSAmt.ActiveSheet.Cells[1, 2].Text = clsPmpaType.RPG.Amt5[2].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[1, 3].Text = clsPmpaType.RPG.Amt6[2].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[1, 4].Text = clsPmpaType.RPG.Amt4[2].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[1, 5].Text = clsPmpaType.RPG.Amt3[2].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[1, 6].Text = clsPmpaType.RPG.Amt2[2].ToString("#,##0 ");    //비급여

            //식대
            SSAmt.ActiveSheet.Cells[2, 2].Text = clsPmpaType.RPG.Amt5[3].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[2, 3].Text = clsPmpaType.RPG.Amt6[3].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[2, 4].Text = clsPmpaType.RPG.Amt4[3].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[2, 5].Text = clsPmpaType.RPG.Amt3[3].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[2, 6].Text = clsPmpaType.RPG.Amt2[3].ToString("#,##0 ");    //비급여

            //투약 조제 행위
            SSAmt.ActiveSheet.Cells[3, 2].Text = clsPmpaType.RPG.Amt5[4].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[3, 3].Text = clsPmpaType.RPG.Amt6[4].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[3, 4].Text = clsPmpaType.RPG.Amt4[4].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[3, 5].Text = clsPmpaType.RPG.Amt3[4].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[3, 6].Text = clsPmpaType.RPG.Amt2[4].ToString("#,##0 ");    //비급여

            //투약 조제 약품
            SSAmt.ActiveSheet.Cells[4, 2].Text = clsPmpaType.RPG.Amt5[5].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[4, 3].Text = clsPmpaType.RPG.Amt6[5].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[4, 4].Text = clsPmpaType.RPG.Amt4[5].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[4, 5].Text = clsPmpaType.RPG.Amt3[5].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[4, 6].Text = clsPmpaType.RPG.Amt2[5].ToString("#,##0 ");    //비급여

            //주사 행위
            SSAmt.ActiveSheet.Cells[5, 2].Text = clsPmpaType.RPG.Amt5[6].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[5, 3].Text = clsPmpaType.RPG.Amt6[6].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[5, 4].Text = clsPmpaType.RPG.Amt4[6].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[5, 5].Text = clsPmpaType.RPG.Amt3[6].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[5, 6].Text = clsPmpaType.RPG.Amt2[6].ToString("#,##0 ");    //비급여

            //주사 약품
            SSAmt.ActiveSheet.Cells[6, 2].Text = clsPmpaType.RPG.Amt5[7].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[6, 3].Text = clsPmpaType.RPG.Amt6[7].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[6, 4].Text = clsPmpaType.RPG.Amt4[7].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[6, 5].Text = clsPmpaType.RPG.Amt3[7].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[6, 6].Text = clsPmpaType.RPG.Amt2[7].ToString("#,##0 ");    //비급여

            //마취
            SSAmt.ActiveSheet.Cells[7, 2].Text = clsPmpaType.RPG.Amt5[8].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[7, 3].Text = clsPmpaType.RPG.Amt6[8].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[7, 4].Text = clsPmpaType.RPG.Amt4[8].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[7, 5].Text = clsPmpaType.RPG.Amt3[8].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[7, 6].Text = clsPmpaType.RPG.Amt2[8].ToString("#,##0 ");    //비급여

            //처치 수술
            SSAmt.ActiveSheet.Cells[8, 2].Text = clsPmpaType.RPG.Amt5[9].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[8, 3].Text = clsPmpaType.RPG.Amt6[9].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[8, 4].Text = clsPmpaType.RPG.Amt4[9].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[8, 5].Text = clsPmpaType.RPG.Amt3[9].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[8, 6].Text = clsPmpaType.RPG.Amt2[9].ToString("#,##0 ");    //비급여

            //검사료
            SSAmt.ActiveSheet.Cells[9, 2].Text = clsPmpaType.RPG.Amt5[10].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[9, 3].Text = clsPmpaType.RPG.Amt6[10].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[9, 4].Text = clsPmpaType.RPG.Amt4[10].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[9, 5].Text = clsPmpaType.RPG.Amt3[10].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[9, 6].Text = clsPmpaType.RPG.Amt2[10].ToString("#,##0 ");    //비급여

            //영상진단
            SSAmt.ActiveSheet.Cells[10, 2].Text = clsPmpaType.RPG.Amt5[11].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[10, 3].Text = clsPmpaType.RPG.Amt6[11].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[10, 4].Text = clsPmpaType.RPG.Amt4[11].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[10, 5].Text = clsPmpaType.RPG.Amt3[11].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[10, 6].Text = clsPmpaType.RPG.Amt2[11].ToString("#,##0 ");    //비급여

            //방사선치료
            SSAmt.ActiveSheet.Cells[11, 2].Text = clsPmpaType.RPG.Amt5[12].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[11, 3].Text = clsPmpaType.RPG.Amt6[12].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[11, 4].Text = clsPmpaType.RPG.Amt4[12].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[11, 5].Text = clsPmpaType.RPG.Amt3[12].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[11, 6].Text = clsPmpaType.RPG.Amt2[12].ToString("#,##0 ");    //비급여

            //치료재료대
            SSAmt.ActiveSheet.Cells[12, 2].Text = clsPmpaType.RPG.Amt5[13].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[12, 3].Text = clsPmpaType.RPG.Amt6[13].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[12, 4].Text = clsPmpaType.RPG.Amt4[13].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[12, 5].Text = clsPmpaType.RPG.Amt3[13].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[12, 6].Text = clsPmpaType.RPG.Amt2[13].ToString("#,##0 ");    //비급여

            //물리치료
            SSAmt.ActiveSheet.Cells[13, 2].Text = clsPmpaType.RPG.Amt5[14].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[13, 3].Text = clsPmpaType.RPG.Amt6[14].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[13, 4].Text = clsPmpaType.RPG.Amt4[14].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[13, 5].Text = clsPmpaType.RPG.Amt3[14].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[13, 6].Text = clsPmpaType.RPG.Amt2[14].ToString("#,##0 ");    //비급여

            //정신요법
            SSAmt.ActiveSheet.Cells[14, 2].Text = clsPmpaType.RPG.Amt5[15].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[14, 3].Text = clsPmpaType.RPG.Amt6[15].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[14, 4].Text = clsPmpaType.RPG.Amt4[15].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[14, 5].Text = clsPmpaType.RPG.Amt3[15].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[14, 6].Text = clsPmpaType.RPG.Amt2[15].ToString("#,##0 ");    //비급여

            //전혈
            SSAmt.ActiveSheet.Cells[15, 2].Text = clsPmpaType.RPG.Amt5[16].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[15, 3].Text = clsPmpaType.RPG.Amt6[16].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[15, 4].Text = clsPmpaType.RPG.Amt4[16].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[15, 5].Text = clsPmpaType.RPG.Amt3[16].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[15, 6].Text = clsPmpaType.RPG.Amt2[16].ToString("#,##0 ");    //비급여

            //CT
            SSAmt.ActiveSheet.Cells[0, 8].Text = clsPmpaType.RPG.Amt5[17].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[0, 9].Text = clsPmpaType.RPG.Amt6[17].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[0, 10].Text = clsPmpaType.RPG.Amt4[17].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[0, 11].Text = clsPmpaType.RPG.Amt3[17].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[0, 12].Text = clsPmpaType.RPG.Amt2[17].ToString("#,##0 ");    //비급여

            //MRI
            SSAmt.ActiveSheet.Cells[1, 8].Text = clsPmpaType.RPG.Amt5[18].ToString("#,##0 ");    ///본인부담(급여)
            SSAmt.ActiveSheet.Cells[1, 9].Text = clsPmpaType.RPG.Amt6[18].ToString("#,##0 ");    ///조합부담(급여)
            SSAmt.ActiveSheet.Cells[1, 10].Text = clsPmpaType.RPG.Amt4[18].ToString("#,##0 ");    ///전액본인
            SSAmt.ActiveSheet.Cells[1, 11].Text = clsPmpaType.RPG.Amt3[18].ToString("#,##0 ");    ///선택진료
            SSAmt.ActiveSheet.Cells[1, 12].Text = clsPmpaType.RPG.Amt2[18].ToString("#,##0 ");    ///비급여

            //초음파
            SSAmt.ActiveSheet.Cells[2, 8].Text = clsPmpaType.RPG.Amt5[19].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[2, 9].Text = clsPmpaType.RPG.Amt6[19].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[2, 10].Text = clsPmpaType.RPG.Amt4[19].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[2, 11].Text = clsPmpaType.RPG.Amt3[19].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[2, 12].Text = clsPmpaType.RPG.Amt2[19].ToString("#,##0 ");    //비급여

            //보철
            SSAmt.ActiveSheet.Cells[3, 8].Text = clsPmpaType.RPG.Amt5[20].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[3, 9].Text = clsPmpaType.RPG.Amt6[20].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[3, 10].Text = clsPmpaType.RPG.Amt4[20].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[3, 11].Text = clsPmpaType.RPG.Amt3[20].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[3, 12].Text = clsPmpaType.RPG.Amt2[20].ToString("#,##0 ");    //비급여

            //증명료
            SSAmt.ActiveSheet.Cells[5, 8].Text = clsPmpaType.RPG.Amt5[22].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[5, 9].Text = clsPmpaType.RPG.Amt6[22].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[5, 10].Text = clsPmpaType.RPG.Amt4[22].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[5, 11].Text = clsPmpaType.RPG.Amt3[22].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[5, 12].Text = clsPmpaType.RPG.Amt2[22].ToString("#,##0 ");    //비급여

            //병실차액
            SSAmt.ActiveSheet.Cells[6, 8].Text = clsPmpaType.RPG.Amt5[21].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[6, 9].Text = clsPmpaType.RPG.Amt6[21].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[6, 10].Text = clsPmpaType.RPG.Amt4[21].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[6, 11].Text = clsPmpaType.RPG.Amt3[21].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[6, 12].Text = clsPmpaType.RPG.Amt2[21].ToString("#,##0 ");    //비급여

            //선별급여
            SSAmt.ActiveSheet.Cells[7, 8].Text = clsPmpaType.RPG.Amt5[24].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[7, 9].Text = clsPmpaType.RPG.Amt6[24].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[7, 10].Text = clsPmpaType.RPG.Amt4[24].ToString("#,##0 ");   //전액본인
            SSAmt.ActiveSheet.Cells[7, 11].Text = clsPmpaType.RPG.Amt3[24].ToString("#,##0 ");   //선택진료
            SSAmt.ActiveSheet.Cells[7, 12].Text = clsPmpaType.RPG.Amt2[24].ToString("#,##0 ");    //비급여

            //기타
            SSAmt.ActiveSheet.Cells[14, 8].Text = clsPmpaType.RPG.Amt5[49].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[14, 9].Text = clsPmpaType.RPG.Amt6[49].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[14, 10].Text = clsPmpaType.RPG.Amt4[49].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[14, 11].Text = clsPmpaType.RPG.Amt3[49].ToString("#,##0 ");    //선택진료
            SSAmt.ActiveSheet.Cells[14, 12].Text = clsPmpaType.RPG.Amt2[49].ToString("#,##0 ");    //비급여
            #endregion

            if (strDrg == "D")
            {
                long nSunAmt_Tot = DRG.GnGs50Amt_T + DRG.GnGs80Amt_T + DRG.GnGs90Amt_T;
                long nSunAmt_Jhp = DRG.GnGs50Amt_J + DRG.GnGs80Amt_J + DRG.GnGs90Amt_J;
                long nSunAmt_BOn = DRG.GnGs50Amt_B + DRG.GnGs80Amt_B + DRG.GnGs90Amt_B;

                //선별급여 표시
                SSAmt.ActiveSheet.Cells[7, 8].Text = nSunAmt_BOn.ToString("#,##0 ");        //본인부담(급여)
                SSAmt.ActiveSheet.Cells[7, 9].Text = nSunAmt_Jhp.ToString("#,##0 ");        //조합부담(급여)
                SSAmt.ActiveSheet.Cells[7, 10].Text = "0";                                  //전액본인
                SSAmt.ActiveSheet.Cells[7, 11].Text = "0";                                  //선택진료
                SSAmt.ActiveSheet.Cells[7, 12].Text = "0";                                  //비급여

                //DRG 금액칸 표시
                if (clsPmpaType.TIT.Amt[70] >= 0)   //DRG 예전버전
                {
                    SSAmt.ActiveSheet.Cells[8, 8].Text = DRG.GnDrgBonAmt.ToString("#,##0 ");    //본인부담(급여)
                    SSAmt.ActiveSheet.Cells[8, 9].Text = DRG.GnDrgJohapAmt.ToString("#,##0 ");    //조합부담(급여)
                    SSAmt.ActiveSheet.Cells[8, 10].Text = "0";  //전액본인
                    SSAmt.ActiveSheet.Cells[8, 11].Text = "0";  //선택진료
                    SSAmt.ActiveSheet.Cells[8, 12].Text = "0";  //비급여
                }
                else
                {
                    //DRG
                    SSAmt.ActiveSheet.Cells[8, 8].Text = clsPmpaType.TIT.Amt[69].ToString("#,##0 ");    //본인부담(급여)
                    SSAmt.ActiveSheet.Cells[8, 9].Text = clsPmpaType.TIT.Amt[68].ToString("#,##0 ");    //조합부담(급여)
                    SSAmt.ActiveSheet.Cells[8, 10].Text = "0";  //전액본인
                    SSAmt.ActiveSheet.Cells[8, 11].Text = "0";  //선택진료
                    SSAmt.ActiveSheet.Cells[8, 12].Text = "0";  //비급여
                    clsPmpaType.RPG.Amt5[50] += clsPmpaType.TIT.Amt[69];
                    clsPmpaType.RPG.Amt6[50] += clsPmpaType.TIT.Amt[68];
                    clsPmpaType.RPG.Amt1[50] += clsPmpaType.TIT.Amt[70];
                }

                long nHapTot = DRG.GnDRG_Amt2 + DRG.GnDrgFoodAmt[0] + DRG.GnDrgFoodAmt[1] + DRG.GnDrgRoomAmt[0] + DRG.GnDrgRoomAmt[1];
                long nHapJhp = DRG.GnDrgJohapAmt + DRG.GnDrgFoodAmt[1] + DRG.GnDrgRoomAmt[1];
                long nHapBon = DRG.GnDrgBonAmt + DRG.GnDrgFoodAmt[0] + DRG.GnDrgRoomAmt[0]; //(nHapTot - nHapJhp);

                nHapTot += nSunAmt_Tot;
                nHapJhp += nSunAmt_Jhp;
                nHapBon += nSunAmt_BOn;

                //합계
                SSAmt.ActiveSheet.Cells[15, 8].Text = nHapBon.ToString("#,##0 ");               //본인부담(급여)
                SSAmt.ActiveSheet.Cells[15, 9].Text = nHapJhp.ToString("#,##0 ");               //조합부담(급여)
                SSAmt.ActiveSheet.Cells[15, 10].Text = DRG.GnGs100Amt.ToString("#,##0 ");       //전액본인
                SSAmt.ActiveSheet.Cells[15, 11].Text = DRG.GnDrgSelTAmt.ToString("#,##0 ");     //선택진료
                SSAmt.ActiveSheet.Cells[15, 12].Text = DRG.GnDrgBiFAmt.ToString("#,##0 ");      //비급여

                //이미 납부한 금액
                if (nAmt[51] > 0)
                {
                    clsPmpaType.TIT.Amt[51] = (long)nAmt[51];
                }

                //보증금
                if (nAmt[52] > 0)
                {
                    SSAmt.ActiveSheet.Cells[10, 14].Text = nAmt[52].ToString("#,##0 ");
                }

                //환자부담총액
                SSAmt.ActiveSheet.Cells[0, 14].Text = clsPmpaType.TIT.Amt[50].ToString("#,##0 ");
                //조합부담금
                SSAmt.ActiveSheet.Cells[1, 14].Text = clsPmpaType.TIT.Amt[53].ToString("#,##0 ");
                //본인부담금
                SSAmt.ActiveSheet.Cells[2, 14].Text = clsPmpaType.TIT.Amt[55].ToString("#,##0 ");
                //이미 납부한 금액
                SSAmt.ActiveSheet.Cells[3, 14].Text = clsPmpaType.TIT.Amt[51].ToString("#,##0 ");
                //할인액
                SSAmt.ActiveSheet.Cells[4, 14].Text = clsPmpaType.TIT.Amt[54].ToString("#,##0 ");
                //미수액
                SSAmt.ActiveSheet.Cells[5, 14].Text = clsPmpaType.TIT.Amt[56].ToString("#,##0 ");
                //대불금
                SSAmt.ActiveSheet.Cells[6, 14].Text = nAmtSang.ToString("#,##0 ");

                //희귀, 난치성 지원금
                if (clsPmpaType.TIT.OgPdBun == "H")
                {
                    SSAmt.ActiveSheet.Cells[7, 14].Text = clsPmpaType.TIT.Amt[62].ToString("#,##0 ");
                }
                else if (clsPmpaType.TIT.VCode == "V206" || clsPmpaType.TIT.VCode == "V231")
                {
                    SSAmt.ActiveSheet.Cells[7, 14].Text = clsPmpaType.TIT.Amt[62].ToString("#,##0 ");
                    //수납액
                    if (DRG.GnDrg지원금 > 0)
                    {
                        SSAmt.ActiveSheet.Cells[17, 14].Text = DRG.GnDrg지원금.ToString("#,##0 ");
                    }
                    else
                    {
                        long nJiwonAmt = (long)Math.Round((clsPmpaType.TIT.Amt[55] - DRG.GnDrgBiTAmt) * 0.5, 0, MidpointRounding.AwayFromZero);

                        SSAmt.ActiveSheet.Cells[15, 14].Text = nJiwonAmt.ToString("#,##0 ");
                    }
                }
                else
                {
                    SSAmt.ActiveSheet.Cells[15, 14].Text = clsPmpaType.TIT.Amt[57].ToString("#,##0 ");
                }
            }
            else
            {
                //합계
                SSAmt.ActiveSheet.Cells[15, 8].Text = (clsPmpaType.RPG.Amt5[50] - clsPmpaType.TIT.Amt[64]).ToString("#,##0 ");    //본인부담(급여)
                SSAmt.ActiveSheet.Cells[15, 9].Text = clsPmpaType.RPG.Amt6[50].ToString("#,##0 ");                                //조합부담(급여)
                SSAmt.ActiveSheet.Cells[15, 10].Text = clsPmpaType.RPG.Amt4[50].ToString("#,##0 ");                                //전액본인
                SSAmt.ActiveSheet.Cells[15, 11].Text = clsPmpaType.RPG.Amt3[50].ToString("#,##0 ");                                //선택진료
                SSAmt.ActiveSheet.Cells[15, 12].Text = clsPmpaType.RPG.Amt2[50].ToString("#,##0 ");                                //비급여

                //급여/비급여 합계            
                SSAmt.ActiveSheet.Cells[11, 14].Text = (clsPmpaType.RPG.Amt1[50] - clsPmpaType.TIT.Amt[64]).ToString("#,##0 ");                             //요양급여합계
                SSAmt.ActiveSheet.Cells[12, 14].Text = clsPmpaType.RPG.Amt6[50].ToString("#,##0 ");                                                         //요양급여조합
                SSAmt.ActiveSheet.Cells[13, 14].Text = clsPmpaType.RPG.Amt5[50].ToString("#,##0 ");                                                         //요양급여본인
                SSAmt.ActiveSheet.Cells[14, 14].Text = (clsPmpaType.RPG.Amt2[50] + clsPmpaType.RPG.Amt3[50] + clsPmpaType.RPG.Amt4[50]).ToString("#,##0 ");  //비급여함

                //이미 납부한 금액
                if (nAmt[51] > 0)
                {
                    clsPmpaType.TIT.Amt[51] = (long)nAmt[51];
                }

                //부가세
                //if (clsPmpaType.TIT.GbTax == "1")
                //{
                //    SSAmt.ActiveSheet.Cells[12, 13].Text = "부가세";
                //    SSAmt.ActiveSheet.Cells[12, 14].Text = clsPmpaType.TIT.Amt[65].ToString("###,###,##0");
                //}

                //보증금
                if (nAmt[52] > 0)
                {
                    SSAmt.ActiveSheet.Cells[10, 14].Text = nAmt[52].ToString("#,##0 ");
                }

                //환자부담총액
                SSAmt.ActiveSheet.Cells[0, 14].Text = clsPmpaType.TIT.RAmt[26, 1].ToString("#,##0 ");
                //조합부담금
                SSAmt.ActiveSheet.Cells[1, 14].Text = clsPmpaType.TIT.RAmt[25, 1].ToString("#,##0 ");
                //본인부담금
                SSAmt.ActiveSheet.Cells[2, 14].Text = clsPmpaType.TIT.RAmt[27, 1].ToString("#,##0 ");
                //이미 납부한 금액
                SSAmt.ActiveSheet.Cells[3, 14].Text = clsPmpaType.TIT.RAmt[28, 1].ToString("#,##0 ");
                //할인액
                SSAmt.ActiveSheet.Cells[4, 14].Text = clsPmpaType.TIT.RAmt[29, 1].ToString("#,##0 ");
                //미수액
                SSAmt.ActiveSheet.Cells[5, 14].Text = clsPmpaType.TIT.RAmt[30, 1].ToString("#,##0 ");
                //대불금
                SSAmt.ActiveSheet.Cells[6, 14].Text = nAmtSang.ToString("#,##0 ");
                //2인실
                SSAmt.ActiveSheet.Cells[0, 16].Text = clsPmpaPb.GnHRoomBonin.ToString("#,##0 ");
                //희귀, 난치성 지원금
                if (strOBPDBun == "H")
                {
                    SSAmt.ActiveSheet.Cells[7, 14].Text = clsPmpaType.TIT.Amt[62].ToString("#,##0 ");
                  
                }
                else if (Convert.ToDateTime(strInDate) >= Convert.ToDateTime("2011-04-01") && (strVCode == "V206" || strVCode == "V231"))
                {
                    if (clsPmpaType.TIT.FCode == "F008")
                    {
                        SSAmt.ActiveSheet.Cells[7, 14].Text = ((((clsPmpaType.TIT.Amt[62] + clsPmpaPb.GnAntiTubeDrug_Amt * (100 / 100)) / 10) + 0.5) * 10).ToString("#,##0 ");
                    }
                    else
                    {
                        SSAmt.ActiveSheet.Cells[7, 14].Text = ((((clsPmpaType.TIT.Amt[62] * (50 / 100)) / 10) + 0.5) * 10).ToString("#,##0 ");
                    }

                    //수납액
                    if (clsPmpaType.TIT.TGbSts == "7")  //VB - GbSTS 였음. 검토 필요
                    {
                        SSAmt.ActiveSheet.Cells[15, 14].Text = clsPmpaType.TIT.RAmt[31, 1].ToString("#,##0 ");
                    }
                    else
                    {
                        //수납액 = 진료비총액 - 공단부담금 - 중간납부 - 할인액 - 미수액 - 보증금 + 부가세
                        clsPmpaType.TIT.RAmt[31, 1] = (long)(clsPmpaType.TIT.RAmt[26, 1] - clsPmpaType.TIT.RAmt[25, 1] - clsPmpaType.TIT.RAmt[28, 1] -
                                                             clsPmpaType.TIT.RAmt[29, 1] - clsPmpaType.TIT.RAmt[30, 1] - nAmt[52] + clsPmpaType.TIT.Amt[65]);
                        SSAmt.ActiveSheet.Cells[15, 14].Text = clsPmpaType.TIT.RAmt[31, 1].ToString("#,##0 ");
                    }
                }
                else if (string.Compare(clsPmpaType.TIT.InDate, "2015-07-01") >= 0 && clsPmpaType.TIT.FCode == "F010")
                {
                    SSAmt.ActiveSheet.Cells[7, 14].Text = ((((clsPmpaType.TIT.Amt[62] + clsPmpaPb.GnAntiTubeDrug_Amt * (100 / 100)) / 10) + 0.5) * 10).ToString("#,##0 ");
                    //수납액
                    if (clsPmpaType.TIT.TGbSts == "7")  //VB - GbSTS 였음. 검토 필요
                    {
                        SSAmt.ActiveSheet.Cells[15, 14].Text = clsPmpaType.TIT.RAmt[31, 1].ToString("#,##0 ");
                    }
                    else
                    {
                        //수납액 = 진료비총액 - 공단부담금 - 중간납부 - 할인액 - 미수액 - 보증금 + 부가세
                        clsPmpaType.TIT.RAmt[31, 1] = (long)(clsPmpaType.TIT.RAmt[26, 1] - clsPmpaType.TIT.RAmt[25, 1] - clsPmpaType.TIT.RAmt[28, 1] -
                                                             clsPmpaType.TIT.RAmt[29, 1] - clsPmpaType.TIT.RAmt[30, 1] - nAmt[52] + clsPmpaType.TIT.Amt[65]);
                        SSAmt.ActiveSheet.Cells[15, 14].Text = clsPmpaType.TIT.RAmt[31, 1].ToString("#,##0 ");
                    }
                }
                else
                {
                    if (clsPmpaType.TIT.TGbSts == "7")  //VB - GbSTS 였음. 검토 필요
                    {
                        SSAmt.ActiveSheet.Cells[15, 14].Text = clsPmpaType.TIT.RAmt[31, 1].ToString("#,##0 ");
                    }
                    else
                    {
                        //수납액 = 진료비총액 - 공단부담금 - 중간납부 - 할인액 - 미수액 - 보증금 + 부가세
                        clsPmpaType.TIT.RAmt[31, 1] = (long)(clsPmpaType.TIT.RAmt[26, 1] - clsPmpaType.TIT.RAmt[25, 1] - clsPmpaType.TIT.RAmt[28, 1] -
                                                             clsPmpaType.TIT.RAmt[29, 1] - clsPmpaType.TIT.RAmt[30, 1] - nAmt[52] + clsPmpaType.TIT.Amt[65]);
                        SSAmt.ActiveSheet.Cells[15, 14].Text = clsPmpaType.TIT.RAmt[31, 1].ToString("#,##0 ");
                    }
                }

                //약제상한차액
              //  SSAmt.ActiveSheet.Cells[10, 14].Text = clsPmpaType.TIT.Amt[51].ToString("#,##0 ");

                if (clsPmpaType.TIT.Amt[64] < 0)
                {
                    ComFunc.MsgBox("약제상한차액 - 금액확인요망!!");
                }
            }

        }

        /// <summary>
        /// Description : 입원진료비 금액 (신)화면표시
        /// Author : 김민철
        /// Create Date : 2018.05.04
        /// </summary>
        /// <param name="SSAmt"></param>
        /// <param name="nAmt"></param>
        /// <param name="nAmtSang"></param>
        /// <param name="strOBPDBun"></param>
        /// <param name="strInDate"></param>
        /// <param name="strVCode"></param>
        void SSAmt_Display_New(FpSpread SSAmt, double[] nAmt, double nAmtSang, string strOBPDBun, string strInDate, string strVCode, string strDrg)
        {
            #region 항목별 금액표시
            //진찰
            SSAmt.ActiveSheet.Cells[0, 2].Text = clsPmpaType.RPG.Amt1[1].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[0, 3].Text = clsPmpaType.RPG.Amt5[1].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[0, 4].Text = clsPmpaType.RPG.Amt6[1].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[0, 5].Text = clsPmpaType.RPG.Amt4[1].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[0, 6].Text = (clsPmpaType.RPG.Amt2[1] + clsPmpaType.RPG.Amt3[1]).ToString("#,##0 ");    //비급여

            //입원
            SSAmt.ActiveSheet.Cells[1, 2].Text = clsPmpaType.RPG.Amt1[2].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[1, 3].Text = clsPmpaType.RPG.Amt5[2].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[1, 4].Text = clsPmpaType.RPG.Amt6[2].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[1, 5].Text = clsPmpaType.RPG.Amt4[2].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[1, 6].Text = (clsPmpaType.RPG.Amt3[2] + clsPmpaType.RPG.Amt2[2]).ToString("#,##0 ");    //비급여

            //식대
            SSAmt.ActiveSheet.Cells[2, 2].Text = clsPmpaType.RPG.Amt1[3].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[2, 3].Text = clsPmpaType.RPG.Amt5[3].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[2, 4].Text = clsPmpaType.RPG.Amt6[3].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[2, 5].Text = clsPmpaType.RPG.Amt4[3].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[2, 6].Text = (clsPmpaType.RPG.Amt3[3] + clsPmpaType.RPG.Amt2[3]).ToString("#,##0 ");    //비급여

            //투약 조제 행위
            SSAmt.ActiveSheet.Cells[3, 2].Text = clsPmpaType.RPG.Amt1[4].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[3, 3].Text = clsPmpaType.RPG.Amt5[4].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[3, 4].Text = clsPmpaType.RPG.Amt6[4].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[3, 5].Text = clsPmpaType.RPG.Amt4[4].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[3, 6].Text = (clsPmpaType.RPG.Amt3[4] + clsPmpaType.RPG.Amt2[4]).ToString("#,##0 ");    //비급여

            //투약 조제 약품
            SSAmt.ActiveSheet.Cells[4, 2].Text = clsPmpaType.RPG.Amt1[5].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[4, 3].Text = clsPmpaType.RPG.Amt5[5].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[4, 4].Text = clsPmpaType.RPG.Amt6[5].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[4, 5].Text = clsPmpaType.RPG.Amt4[5].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[4, 6].Text = (clsPmpaType.RPG.Amt3[5] + clsPmpaType.RPG.Amt2[5]).ToString("#,##0 ");    //비급여

            //주사 행위
            SSAmt.ActiveSheet.Cells[5, 2].Text = clsPmpaType.RPG.Amt1[6].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[5, 3].Text = clsPmpaType.RPG.Amt5[6].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[5, 4].Text = clsPmpaType.RPG.Amt6[6].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[5, 5].Text = clsPmpaType.RPG.Amt4[6].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[5, 6].Text = (clsPmpaType.RPG.Amt3[6] + clsPmpaType.RPG.Amt2[6]).ToString("#,##0 ");    //비급여

            //주사 약품
            SSAmt.ActiveSheet.Cells[6, 2].Text = clsPmpaType.RPG.Amt1[7].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[6, 3].Text = clsPmpaType.RPG.Amt5[7].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[6, 4].Text = clsPmpaType.RPG.Amt6[7].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[6, 5].Text = clsPmpaType.RPG.Amt4[7].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[6, 6].Text = (clsPmpaType.RPG.Amt3[7] + clsPmpaType.RPG.Amt2[7]).ToString("#,##0 ");    //비급여

            //마취
            SSAmt.ActiveSheet.Cells[7, 2].Text = clsPmpaType.RPG.Amt1[8].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[7, 3].Text = clsPmpaType.RPG.Amt5[8].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[7, 4].Text = clsPmpaType.RPG.Amt6[8].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[7, 5].Text = clsPmpaType.RPG.Amt4[8].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[7, 6].Text = (clsPmpaType.RPG.Amt3[8] + clsPmpaType.RPG.Amt2[8]).ToString("#,##0 ");    //비급여

            //처치 수술
            SSAmt.ActiveSheet.Cells[8, 2].Text = clsPmpaType.RPG.Amt1[9].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[8, 3].Text = clsPmpaType.RPG.Amt5[9].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[8, 4].Text = clsPmpaType.RPG.Amt6[9].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[8, 5].Text = clsPmpaType.RPG.Amt4[9].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[8, 6].Text = (clsPmpaType.RPG.Amt3[9] + clsPmpaType.RPG.Amt2[9]).ToString("#,##0 ");    //비급여

            //검사료
            SSAmt.ActiveSheet.Cells[9, 2].Text = clsPmpaType.RPG.Amt1[10].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[9, 3].Text = clsPmpaType.RPG.Amt5[10].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[9, 4].Text = clsPmpaType.RPG.Amt6[10].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[9, 5].Text = clsPmpaType.RPG.Amt4[10].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[9, 6].Text = (clsPmpaType.RPG.Amt3[10] + clsPmpaType.RPG.Amt2[10]).ToString("#,##0 ");    //비급여

            //영상진단
            SSAmt.ActiveSheet.Cells[10, 2].Text = clsPmpaType.RPG.Amt1[11].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[10, 3].Text = clsPmpaType.RPG.Amt5[11].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[10, 4].Text = clsPmpaType.RPG.Amt6[11].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[10, 5].Text = clsPmpaType.RPG.Amt4[11].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[10, 6].Text = (clsPmpaType.RPG.Amt3[11] + clsPmpaType.RPG.Amt2[11]).ToString("#,##0 ");    //비급여

            //방사선치료
            SSAmt.ActiveSheet.Cells[11, 2].Text = clsPmpaType.RPG.Amt1[12].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[11, 3].Text = clsPmpaType.RPG.Amt5[12].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[11, 4].Text = clsPmpaType.RPG.Amt6[12].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[11, 5].Text = clsPmpaType.RPG.Amt4[12].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[11, 6].Text = (clsPmpaType.RPG.Amt3[12] + clsPmpaType.RPG.Amt2[12]).ToString("#,##0 ");    //비급여

            //치료재료대
            SSAmt.ActiveSheet.Cells[12, 2].Text = clsPmpaType.RPG.Amt1[13].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[12, 3].Text = clsPmpaType.RPG.Amt5[13].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[12, 4].Text = clsPmpaType.RPG.Amt6[13].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[12, 5].Text = clsPmpaType.RPG.Amt4[13].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[12, 6].Text = (clsPmpaType.RPG.Amt3[13] + clsPmpaType.RPG.Amt2[13]).ToString("#,##0 ");    //비급여

            //물리치료
            SSAmt.ActiveSheet.Cells[13, 2].Text = clsPmpaType.RPG.Amt1[14].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[13, 3].Text = clsPmpaType.RPG.Amt5[14].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[13, 4].Text = clsPmpaType.RPG.Amt6[14].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[13, 5].Text = clsPmpaType.RPG.Amt4[14].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[13, 6].Text = (clsPmpaType.RPG.Amt3[14] + clsPmpaType.RPG.Amt2[14]).ToString("#,##0 ");    //비급여

            //정신요법
            SSAmt.ActiveSheet.Cells[14, 2].Text = clsPmpaType.RPG.Amt1[15].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[14, 3].Text = clsPmpaType.RPG.Amt5[15].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[14, 4].Text = clsPmpaType.RPG.Amt6[15].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[14, 5].Text = clsPmpaType.RPG.Amt4[15].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[14, 6].Text = (clsPmpaType.RPG.Amt3[15] + clsPmpaType.RPG.Amt2[15]).ToString("#,##0 ");    //비급여

            //전혈
            SSAmt.ActiveSheet.Cells[15, 2].Text = clsPmpaType.RPG.Amt1[16].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[15, 3].Text = clsPmpaType.RPG.Amt5[16].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[15, 4].Text = clsPmpaType.RPG.Amt6[16].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[15, 5].Text = clsPmpaType.RPG.Amt4[16].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[15, 6].Text = (clsPmpaType.RPG.Amt3[16] + clsPmpaType.RPG.Amt2[16]).ToString("#,##0 ");    //비급여

            //CT
            SSAmt.ActiveSheet.Cells[0, 8].Text = clsPmpaType.RPG.Amt1[17].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[0, 9].Text = clsPmpaType.RPG.Amt5[17].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[0, 10].Text = clsPmpaType.RPG.Amt6[17].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[0, 11].Text = clsPmpaType.RPG.Amt4[17].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[0, 12].Text = (clsPmpaType.RPG.Amt3[17] + clsPmpaType.RPG.Amt2[17]).ToString("#,##0 ");    //비급여

            //MRI
            SSAmt.ActiveSheet.Cells[1, 8].Text = clsPmpaType.RPG.Amt1[18].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[1, 9].Text = clsPmpaType.RPG.Amt5[18].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[1, 10].Text = clsPmpaType.RPG.Amt6[18].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[1, 11].Text = clsPmpaType.RPG.Amt4[18].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[1, 12].Text = (clsPmpaType.RPG.Amt3[18] + clsPmpaType.RPG.Amt2[18]).ToString("#,##0 ");    //비급여

            //초음파
            SSAmt.ActiveSheet.Cells[2, 8].Text = clsPmpaType.RPG.Amt1[19].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[2, 9].Text = clsPmpaType.RPG.Amt5[19].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[2, 10].Text = clsPmpaType.RPG.Amt6[19].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[2, 11].Text = clsPmpaType.RPG.Amt4[19].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[2, 12].Text = (clsPmpaType.RPG.Amt3[19] + clsPmpaType.RPG.Amt2[19]).ToString("#,##0 ");    //비급여

            //보철
            SSAmt.ActiveSheet.Cells[3, 8].Text = clsPmpaType.RPG.Amt1[20].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[3, 9].Text = clsPmpaType.RPG.Amt5[20].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[3, 10].Text = clsPmpaType.RPG.Amt6[20].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[3, 11].Text = clsPmpaType.RPG.Amt4[20].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[3, 12].Text = (clsPmpaType.RPG.Amt3[20] + clsPmpaType.RPG.Amt2[20]).ToString("#,##0 ");    //비급여

            //증명료
            SSAmt.ActiveSheet.Cells[5, 8].Text = clsPmpaType.RPG.Amt1[22].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[5, 9].Text = clsPmpaType.RPG.Amt5[22].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[5, 10].Text = clsPmpaType.RPG.Amt6[22].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[5, 11].Text = clsPmpaType.RPG.Amt4[22].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[5, 12].Text = (clsPmpaType.RPG.Amt3[22] + clsPmpaType.RPG.Amt2[22]).ToString("#,##0 ");    //비급여

            //병실차액
            SSAmt.ActiveSheet.Cells[6, 8].Text = clsPmpaType.RPG.Amt1[21].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[6, 9].Text = clsPmpaType.RPG.Amt5[21].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[6, 10].Text = clsPmpaType.RPG.Amt6[21].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[6, 11].Text = clsPmpaType.RPG.Amt4[21].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[6, 12].Text = (clsPmpaType.RPG.Amt3[21] + clsPmpaType.RPG.Amt2[21]).ToString("#,##0 ");    //비급여

            //선별급여
            SSAmt.ActiveSheet.Cells[7, 8].Text = clsPmpaType.RPG.Amt1[24].ToString("#,##0 ");     //본인부담(급여)
            SSAmt.ActiveSheet.Cells[7, 9].Text = clsPmpaType.RPG.Amt5[24].ToString("#,##0 ");     //본인부담(급여)
            SSAmt.ActiveSheet.Cells[7, 10].Text = clsPmpaType.RPG.Amt6[24].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[7, 11].Text = clsPmpaType.RPG.Amt4[24].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[7, 12].Text = (clsPmpaType.RPG.Amt3[24] + clsPmpaType.RPG.Amt2[24]).ToString("#,##0 ");    //비급여

            //기타
            SSAmt.ActiveSheet.Cells[14, 8].Text = clsPmpaType.RPG.Amt1[49].ToString("#,##0 ");    //총부담(급여)
            SSAmt.ActiveSheet.Cells[14, 9].Text = clsPmpaType.RPG.Amt5[49].ToString("#,##0 ");    //본인부담(급여)
            SSAmt.ActiveSheet.Cells[14, 10].Text = clsPmpaType.RPG.Amt6[49].ToString("#,##0 ");    //조합부담(급여)
            SSAmt.ActiveSheet.Cells[14, 11].Text = clsPmpaType.RPG.Amt4[49].ToString("#,##0 ");    //전액본인
            SSAmt.ActiveSheet.Cells[14, 12].Text = (clsPmpaType.RPG.Amt3[49] + clsPmpaType.RPG.Amt2[49]).ToString("#,##0 ");    //비급여
            #endregion

            if (strDrg == "D")
            {
                long nSunAmt_Tot = DRG.GnGs50Amt_T + DRG.GnGs80Amt_T + DRG.GnGs90Amt_T;
                long nSunAmt_Jhp = DRG.GnGs50Amt_J + DRG.GnGs80Amt_J + DRG.GnGs90Amt_J;
                long nSunAmt_Bon = DRG.GnGs50Amt_B + DRG.GnGs80Amt_B + DRG.GnGs90Amt_B;

                //선별급여 표시
                SSAmt.ActiveSheet.Cells[7, 8].Text = nSunAmt_Tot.ToString("#,##0 ");
                SSAmt.ActiveSheet.Cells[7, 9].Text = nSunAmt_Bon.ToString("#,##0 ");
                SSAmt.ActiveSheet.Cells[7, 10].Text = nSunAmt_Jhp.ToString("#,##0 ");
                SSAmt.ActiveSheet.Cells[7, 11].Text = "0";
                SSAmt.ActiveSheet.Cells[7, 12].Text = "0";

                //DRG 금액칸 표시
                if (clsPmpaType.TIT.Amt[70] >= 0)   //DRG 예전버전
                {
                    SSAmt.ActiveSheet.Cells[8, 8].Text = DRG.GnDRG_Amt2.ToString("#,##0 ");
                    SSAmt.ActiveSheet.Cells[8, 9].Text = DRG.GnDrgBonAmt.ToString("#,##0 ");
                    SSAmt.ActiveSheet.Cells[8, 10].Text = DRG.GnDrgJohapAmt.ToString("#,##0 ");
                    SSAmt.ActiveSheet.Cells[8, 11].Text = "0";
                    SSAmt.ActiveSheet.Cells[8, 12].Text = "0";
                }
                else
                {
                    //DRG
                    SSAmt.ActiveSheet.Cells[8, 8].Text = clsPmpaType.TIT.Amt[91].ToString("#,##0 ");
                    SSAmt.ActiveSheet.Cells[8, 9].Text = clsPmpaType.TIT.Amt[69].ToString("#,##0 ");
                    SSAmt.ActiveSheet.Cells[8, 10].Text = clsPmpaType.TIT.Amt[68].ToString("#,##0 ");
                    SSAmt.ActiveSheet.Cells[8, 11].Text = "0";
                    SSAmt.ActiveSheet.Cells[8, 12].Text = "0";
                }

                long nHapTot = DRG.GnDRG_Amt2 + DRG.GnDrgFoodAmt[0] + DRG.GnDrgFoodAmt[1] + DRG.GnDrgRoomAmt[0] + DRG.GnDrgRoomAmt[1];
                long nHapJhp = DRG.GnDrgJohapAmt + DRG.GnDrgFoodAmt[1] + DRG.GnDrgRoomAmt[1];
                long nHapBon = DRG.GnDrgBonAmt + DRG.GnDrgFoodAmt[0] + DRG.GnDrgRoomAmt[0]; //(nHapTot - nHapJhp);

                nHapTot += nSunAmt_Tot;
                nHapJhp += nSunAmt_Jhp;
                nHapBon += nSunAmt_Bon;

                //합계
                SSAmt.ActiveSheet.Cells[15, 8].Text = nHapTot.ToString("#,##0 ");               //총부담(급여)
                SSAmt.ActiveSheet.Cells[15, 9].Text = nHapBon.ToString("#,##0 ");               //본인부담(급여)
                SSAmt.ActiveSheet.Cells[15, 10].Text = nHapJhp.ToString("#,##0 ");              //조합부담(급여)
                SSAmt.ActiveSheet.Cells[15, 11].Text = DRG.GnGs100Amt.ToString("#,##0 ");       //전액본인
                SSAmt.ActiveSheet.Cells[15, 12].Text = (DRG.GnDrgBiFAmt + DRG.GnDrgSelTAmt).ToString("#,##0 ");     //비급여      

                //보증금
                if (nAmt[52] > 0)
                {
                    SSAmt.ActiveSheet.Cells[10, 14].Text = nAmt[52].ToString("#,##0 ");
                }

                //환자부담총액
                SSAmt.ActiveSheet.Cells[0, 14].Text = clsPmpaType.TIT.Amt[50].ToString("#,##0 ");
                //조합부담금
                SSAmt.ActiveSheet.Cells[1, 14].Text = clsPmpaType.TIT.Amt[53].ToString("#,##0 ");
                //본인부담금
                SSAmt.ActiveSheet.Cells[2, 14].Text = clsPmpaType.TIT.Amt[55].ToString("#,##0 ");
                //이미 납부한 금액
                SSAmt.ActiveSheet.Cells[3, 14].Text = clsPmpaType.TIT.Amt[51].ToString("#,##0 ");
                //할인액
                SSAmt.ActiveSheet.Cells[4, 14].Text = clsPmpaType.TIT.Amt[54].ToString("#,##0 ");
                //미수액
                SSAmt.ActiveSheet.Cells[5, 14].Text = clsPmpaType.TIT.Amt[56].ToString("#,##0 ");
                //대불금
                SSAmt.ActiveSheet.Cells[6, 14].Text = nAmtSang.ToString("#,##0 ");
                
                //희귀, 난치성 지원금
                if (strOBPDBun == "H")
                {
                    SSAmt.ActiveSheet.Cells[7, 14].Text = clsPmpaType.TIT.Amt[61].ToString("#,##0 ");
                    //수납액
                
                }
                else if (clsPmpaType.TIT.VCode == "V206" || clsPmpaType.TIT.VCode == "V231")
                {
                    SSAmt.ActiveSheet.Cells[7, 14].Text = clsPmpaType.TIT.Amt[62].ToString("#,##0 ");
                    //수납액
                    if (DRG.GnDrg지원금 > 0)
                    {
                        SSAmt.ActiveSheet.Cells[17, 14].Text = DRG.GnDrg지원금.ToString("#,##0 ");
                    }
                    else
                    {
                        long nJiwonAmt = (long)Math.Round((clsPmpaType.TIT.Amt[55] - DRG.GnDrgBiTAmt) * 0.5, 0, MidpointRounding.AwayFromZero);

                        SSAmt.ActiveSheet.Cells[15, 14].Text = nJiwonAmt.ToString("#,##0 ");
                    }
                }
                else
                {
                    SSAmt.ActiveSheet.Cells[15, 14].Text = clsPmpaType.TIT.Amt[57].ToString("#,##0 ");
                }
            }
            else
            {
                //합계
                SSAmt.ActiveSheet.Cells[15, 8].Text = clsPmpaType.RPG.Amt1[50].ToString("#,##0 ");    //총부담(급여)
                SSAmt.ActiveSheet.Cells[15, 9].Text = (clsPmpaType.RPG.Amt5[50] - clsPmpaType.TIT.Amt[64]).ToString("#,##0 ");    //본인부담(급여)
                SSAmt.ActiveSheet.Cells[15, 10].Text = clsPmpaType.RPG.Amt6[50].ToString("#,##0 ");                                //조합부담(급여)
                SSAmt.ActiveSheet.Cells[15, 11].Text = clsPmpaType.RPG.Amt4[50].ToString("#,##0 ");                                //전액본인
                SSAmt.ActiveSheet.Cells[15, 12].Text = (clsPmpaType.RPG.Amt3[50] + clsPmpaType.RPG.Amt2[50]).ToString("#,##0 ");   //비급여

                //급여/비급여 합계            
                SSAmt.ActiveSheet.Cells[11, 14].Text = (clsPmpaType.RPG.Amt1[50] - clsPmpaType.TIT.Amt[64]).ToString("#,##0 ");                             //요양급여합계
                SSAmt.ActiveSheet.Cells[12, 14].Text = clsPmpaType.RPG.Amt6[50].ToString("#,##0 ");                                                         //요양급여조합
                SSAmt.ActiveSheet.Cells[13, 14].Text = clsPmpaType.RPG.Amt5[50].ToString("#,##0 ");                                                         //요양급여본인
                SSAmt.ActiveSheet.Cells[14, 14].Text = (clsPmpaType.RPG.Amt2[50] + clsPmpaType.RPG.Amt3[50] + clsPmpaType.RPG.Amt4[50]).ToString("#,##0 ");  //비급여함

                //부가세
                //if (clsPmpaType.TIT.GbTax == "1")
                //{
                //    SSAmt.ActiveSheet.Cells[12, 13].Text = "부가세";
                //    SSAmt.ActiveSheet.Cells[12, 14].Text = clsPmpaType.TIT.Amt[65].ToString("###,###,##0");
                //}

                //보증금
                if (nAmt[52] > 0)
                {
                    SSAmt.ActiveSheet.Cells[10, 14].Text = nAmt[52].ToString("#,##0 ");
                }

                //환자부담총액
                SSAmt.ActiveSheet.Cells[0, 14].Text = clsPmpaType.TIT.RAmt[26, 1].ToString("#,##0 ");
                //조합부담금
                SSAmt.ActiveSheet.Cells[1, 14].Text = clsPmpaType.TIT.RAmt[25, 1].ToString("#,##0 ");
                //본인부담금
                SSAmt.ActiveSheet.Cells[2, 14].Text = clsPmpaType.TIT.RAmt[27, 1].ToString("#,##0 ");
                //이미 납부한 금액
                SSAmt.ActiveSheet.Cells[3, 14].Text = clsPmpaType.TIT.RAmt[28, 1].ToString("#,##0 ");
                //할인액
                SSAmt.ActiveSheet.Cells[4, 14].Text = clsPmpaType.TIT.RAmt[29, 1].ToString("#,##0 ");
                //미수액
                SSAmt.ActiveSheet.Cells[5, 14].Text = clsPmpaType.TIT.RAmt[30, 1].ToString("#,##0 ");
                //대불금
                SSAmt.ActiveSheet.Cells[6, 14].Text = nAmtSang.ToString("#,##0 ");
                //2인실
                SSAmt.ActiveSheet.Cells[0, 16].Text = clsPmpaPb.GnHRoomBonin.ToString("#,##0 ");

                //희귀, 난치성 지원금
                if (strOBPDBun == "H")
                {
                    SSAmt.ActiveSheet.Cells[7, 14].Text = clsPmpaType.TIT.Amt[62].ToString("#,##0 ");
                    //수납액
                    if (clsPmpaType.TIT.TGbSts == "7")  //VB - GbSTS 였음. 검토 필요
                    {
                        SSAmt.ActiveSheet.Cells[15, 14].Text = clsPmpaType.TIT.RAmt[31, 1].ToString("#,##0 ");
                    }
                    else
                    {
                        clsPmpaType.TIT.RAmt[31, 1] = (long)(clsPmpaType.TIT.RAmt[26, 1] - clsPmpaType.TIT.RAmt[25, 1] - clsPmpaType.TIT.RAmt[28, 1] -
                                                      clsPmpaType.TIT.RAmt[29, 1] - clsPmpaType.TIT.RAmt[30, 1] - nAmt[52] + clsPmpaType.TIT.Amt[65]);
                        SSAmt.ActiveSheet.Cells[15, 14].Text = clsPmpaType.TIT.RAmt[31, 1].ToString("#,##0 ");
                    }
                }
                else if (Convert.ToDateTime(strInDate) >= Convert.ToDateTime("2011-04-01") && (strVCode == "V206" || strVCode == "V231"))
                {
                    if (clsPmpaType.TIT.FCode == "F008")
                    {
                        SSAmt.ActiveSheet.Cells[7, 14].Text = ((((clsPmpaType.TIT.Amt[62] + clsPmpaPb.GnAntiTubeDrug_Amt * (100 / 100)) / 10) + 0.5) * 10).ToString("#,##0 ");
                    }
                    else
                    {
                        SSAmt.ActiveSheet.Cells[7, 14].Text = ((((clsPmpaType.TIT.Amt[62] * (50 / 100)) / 10) + 0.5) * 10).ToString("#,##0 ");
                    }

                    //수납액
                    if (clsPmpaType.TIT.TGbSts == "7")  //VB - GbSTS 였음. 검토 필요
                    {
                        SSAmt.ActiveSheet.Cells[15, 14].Text = clsPmpaType.TIT.RAmt[31, 1].ToString("#,##0 ");
                    }
                    else
                    {
                        //수납액 = 진료비총액 - 공단부담금 - 중간납부 - 할인액 - 미수액 - 보증금 + 부가세
                        clsPmpaType.TIT.RAmt[31, 1] = (long)(clsPmpaType.TIT.RAmt[26, 1] - clsPmpaType.TIT.RAmt[25, 1] - clsPmpaType.TIT.RAmt[28, 1] -
                                                             clsPmpaType.TIT.RAmt[29, 1] - clsPmpaType.TIT.RAmt[30, 1] - nAmt[52] + clsPmpaType.TIT.Amt[65]);
                        SSAmt.ActiveSheet.Cells[15, 14].Text = clsPmpaType.TIT.RAmt[31, 1].ToString("#,##0 ");
                    }
                }
                else if (string.Compare(clsPmpaType.TIT.InDate, "2015-07-01") >= 0 && clsPmpaType.TIT.FCode == "F010")
                {
                    SSAmt.ActiveSheet.Cells[7, 14].Text =((((clsPmpaType.TIT.Amt[62] + clsPmpaPb.GnAntiTubeDrug_Amt * (100 / 100)) / 10) + 0.5) * 10).ToString("#,##0 ");
                    //수납액
                    if (clsPmpaType.TIT.TGbSts == "7")  //VB - GbSTS 였음. 검토 필요
                    {
                        SSAmt.ActiveSheet.Cells[15, 14].Text = clsPmpaType.TIT.RAmt[31, 1].ToString("#,##0 ");
                    }
                    else
                    {
                        //수납액 = 진료비총액 - 공단부담금 - 중간납부 - 할인액 - 미수액 - 보증금 + 부가세
                        clsPmpaType.TIT.RAmt[31, 1] = (long)(clsPmpaType.TIT.RAmt[26, 1] - clsPmpaType.TIT.RAmt[25, 1] - clsPmpaType.TIT.RAmt[28, 1] -
                                                             clsPmpaType.TIT.RAmt[29, 1] - clsPmpaType.TIT.RAmt[30, 1] - nAmt[52] + clsPmpaType.TIT.Amt[65]);
                        SSAmt.ActiveSheet.Cells[15, 14].Text = clsPmpaType.TIT.RAmt[31, 1].ToString("#,##0 ");
                    }
                }
                else
                {
                    if (clsPmpaType.TIT.TGbSts == "7")  //VB - GbSTS 였음. 검토 필요
                    {
                        SSAmt.ActiveSheet.Cells[15, 14].Text = clsPmpaType.TIT.RAmt[31, 1].ToString("#,##0 ");
                    }
                    else
                    {
                        //수납액 = 진료비총액 - 공단부담금 - 중간납부 - 할인액 - 미수액 - 보증금 + 부가세
                        clsPmpaType.TIT.RAmt[31, 1] = (long)(clsPmpaType.TIT.RAmt[26, 1] - clsPmpaType.TIT.RAmt[25, 1] - clsPmpaType.TIT.RAmt[28, 1] -
                                                             clsPmpaType.TIT.RAmt[29, 1] - clsPmpaType.TIT.RAmt[30, 1] - nAmt[52] + clsPmpaType.TIT.Amt[65]);
                        SSAmt.ActiveSheet.Cells[15, 14].Text = clsPmpaType.TIT.RAmt[31, 1].ToString("#,##0 ");
                    }
                }

                //약제상한차액
               // SSAmt.ActiveSheet.Cells[10, 14].Text = clsPmpaType.TIT.Amt[64].ToString("#,##0 ");

                if (clsPmpaType.TIT.Amt[64] < 0)
                {
                    ComFunc.MsgBox("약제상한차액 - 금액확인요망!!");
                }
            }
        }

        /// <summary>
        /// Description : IPD_NEW_SLIP 읽어서 계산용 변수 세팅하기 RPG.AMT
        /// TO_BE 에서 IPD_TRANS_PRTAmt_READ => IPD_TRANS_PRTAmt_READ_NEW 로 변경됨 
        /// IPD_TRANS_PRTAmt_READ, IPD_TRANS_PRTAmt_READ_NEW 로 정의된 폼은 모두 Ipd_Trans_PrtAmt_Read 로 치환하여 사용
        /// Author : 김민철
        /// Create Date : 2017.09.25
        /// </summary>
        /// <param name="ArgTRSNO"></param>
        /// <seealso cref="Report_Print2.bas : IPD_TRANS_PRTAmt_READ_NEW "/>
        public void Ipd_Trans_PrtAmt_Read(PsmhDb pDbCon, long ArgTRSNO, string strTemp, [Optional] string strHUbyAct)
        {
            int i = 0, j = 0;
            int nX = 0, nY = 0;
            int nNu = 0, nSelf = 0;
            string strBun = "", strSuS = "";

            long nAmt1 = 0, nAmt2 = 0;
            long n100BAmt = 0, n100JAmt = 0;

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = "";

            #region Variable Clear
            for (i = 1; i <= 100; i++)
            {
                for (j = 0; j < 7; j++)
                {
                    clsPmpaPb.R2Amt[i, j] = 0;
                }
            }

            clsPmpaType.RPG.Amt1 = new long[51];
            clsPmpaType.RPG.Amt2 = new long[51];
            clsPmpaType.RPG.Amt3 = new long[51];
            clsPmpaType.RPG.Amt4 = new long[51];
            clsPmpaType.RPG.Amt5 = new long[51];
            clsPmpaType.RPG.Amt6 = new long[51];
            clsPmpaType.RPG.Amt7 = new long[51];
            clsPmpaType.RPG.Amt8 = new long[51];
            clsPmpaType.RPG.Amt9 = new long[51];

            //항목별 합
            for (i = 0; i < 51; i++)
            {
                clsPmpaType.RPG.Amt1[i] = 0;  //급여합
                clsPmpaType.RPG.Amt2[i] = 0;  //비급여합
                clsPmpaType.RPG.Amt3[i] = 0;  //특진합
                clsPmpaType.RPG.Amt4[i] = 0;  //본인총액합
                clsPmpaType.RPG.Amt5[i] = 0;  //본인
                clsPmpaType.RPG.Amt6[i] = 0;  //공단
                clsPmpaType.RPG.Amt7[i] = 0;  //선별급여 총액
                clsPmpaType.RPG.Amt8[i] = 0;  //선별급여 조합
                clsPmpaType.RPG.Amt9[i] = 0;  //선별급여 본인
            }
            #endregion

            SQL = "";

            //2021-06-14 호스피스 자격 행위별로 조회 할 때
            if (strHUbyAct == "1" && clsPmpaType.TIT.GBHU == "Y")
            {
                SQL += ComNum.VBLF + " SELECT NU, Bun, GbSelf, SUM(Amt1 * -1) Amt, SUM(Amt2 * -1) Amt2, GbSugbS            ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                 ";
                SQL += ComNum.VBLF + "  WHERE TRSNO = " + ArgTRSNO + "                                           ";
                SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
            }
            else
            {
                SQL += ComNum.VBLF + " SELECT NU, Bun, GbSelf, SUM(Amt1) Amt, SUM(Amt2) Amt2, GbSugbS            ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                 ";
                SQL += ComNum.VBLF + "  WHERE TRSNO = " + ArgTRSNO + "                                           ";
            }
            if (strTemp == "임시자격")
            {
                SQL += ComNum.VBLF + "    AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD') ";
            }

            SQL += ComNum.VBLF + "  GROUP BY Nu,Bun,GbSelf,GbSugbS                                           ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            for (i = 0; i < Dt.Rows.Count; i++)
            {
                nNu = Convert.ToInt16(Dt.Rows[i]["NU"].ToString());
                strBun = Dt.Rows[i]["Bun"].ToString().Trim();
                strSuS = Dt.Rows[i]["GbSugbS"].ToString().Trim();  //100/100

                nAmt1 = Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString()));      //금액1
                nAmt2 = Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt2"].ToString()));     //특진

                if (strSuS == "4" || strSuS == "6")
                {
                    n100BAmt = (long)Math.Round((nAmt1 * 0.8), 0, MidpointRounding.AwayFromZero);
                    n100JAmt = nAmt1 - (long)Math.Round((nAmt1 * 0.8), 0, MidpointRounding.AwayFromZero);
                }
                else if (strSuS == "3" )
                {
                    n100BAmt = (long)Math.Round((nAmt1 * 0.3), 0, MidpointRounding.AwayFromZero);
                    n100JAmt = nAmt1 - (long)Math.Round((nAmt1 * 0.3), 0, MidpointRounding.AwayFromZero);
                }
                else if (strSuS == "2")
                {
                    n100BAmt = (long)Math.Round((nAmt1 * 0.2), 0, MidpointRounding.AwayFromZero);
                    n100JAmt = nAmt1 - (long)Math.Round((nAmt1 * 0.2), 0, MidpointRounding.AwayFromZero);
                }
                else if (strSuS == "5" || strSuS == "7")
                {
                    n100BAmt = (long)Math.Round((nAmt1 * 0.5), 0, MidpointRounding.AwayFromZero);
                    n100JAmt = nAmt1 - (long)Math.Round((nAmt1 * 0.5), 0, MidpointRounding.AwayFromZero);
                }
                else if (strSuS == "8" || strSuS == "9")
                {
                    n100BAmt = (long)Math.Round((nAmt1 * 0.9), 0, MidpointRounding.AwayFromZero);
                    n100JAmt = nAmt1 - (long)Math.Round((nAmt1 * 0.9), 0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    n100BAmt = 0;
                    n100JAmt = 0;
                }

                switch (Dt.Rows[i]["GbSelf"].ToString().Trim())
                {
                    case "0":
                        nSelf = 0;
                        if (strSuS == "2" || strSuS == "3" ||  strSuS == "4" || strSuS == "5" || strSuS == "6" || strSuS == "7" || strSuS == "8" || strSuS == "9")
                            nSelf = 2;
                        break;
                    case "1":
                        nSelf = 1;
                        break;
                    case "2":
                        nSelf = 2;
                        if (strSuS == "1")
                            nSelf = 2;
                        else
                            nSelf = 1;
                        break;
                    default:
                        nSelf = 0;
                        break;
                }

                nY = 0;

                #region //변수배열 분류
                if (nNu == 1)
                {
                    nX = 1;     //진찰료
                }
                else if (nNu == 2 || nNu == 3 || nNu == 21)
                {
                    nX = 2;     //입원료 2
                }
                else if (nNu == 16 || nNu == 34)
                {
                    nX = 3;     //식대3
                }
                else if (nNu == 4 || nNu == 22)
                {
                    if (strBun == "11" || strBun == "12")
                    {
                        nX = 5;         //투약 4
                        nY = 71;        //투약 4
                    }
                    else
                    {
                        nX = 4;         //투약 4
                        nY = 70;        //투약 4
                    }
                }
                else if (nNu == 5 || nNu == 23)
                {
                    if (strBun == "20" || strBun == "21")
                    {
                        nX = 7;         //주사 6
                        nY = 73;        //주사 6
                    }
                    else
                    {
                        nX = 6;         //주사 6
                        nY = 72;        //주사 6
                    }
                }
                else if (nNu == 6 || nNu == 24)
                {
                    nX = 8;     //마취료 8
                }
                else if (nNu == 9 || nNu == 10 || nNu == 12 || nNu == 27 || nNu == 28 || nNu == 30)
                {
                    nX = 9;     //처치 9
                }
                else if (nNu == 13 || nNu == 14 || nNu == 31 || nNu == 32)
                {
                    nX = 10;    //검사 10
                }
                else if (nNu == 15 || nNu == 33)
                {
                    nX = 11;                                //영상진단 11
                    if (strBun == "65")
                        nY = 74;
                    else
                        nY = 75;
                }
                else if (nNu == 7 || nNu == 25)
                {
                    nX = 14;                                //물리치료 14
                }
                else if (nNu == 8 || nNu == 26)
                {
                    nX = 15;                                //정신요법 15
                }
                else if (nNu == 11 || nNu == 29)
                {
                    nX = 16;                                //수혈료 16
                }
                else if (nNu == 19 || nNu == 37)
                {
                    nX = 17;                                //CT 17
                }
                else if (nNu == 18 || nNu == 38)
                {
                    nX = 18;                                //MRI 18
                }
                else if (nNu == 36)
                {
                    nX = 19;                                //초음파 19
                }
                else if (nNu == 40)
                {
                    nX = 20;                                //보철교정 20
                }
                else if (nNu == 35)
                {
                    nX = 21;                                //병실차액 21
                }
                else if (nNu == 47)
                {
                    nX = 22;                                //증명료 22
                }
                else if (nNu == 17 || nNu == 20 || nNu == 39 || nNu == 41 || nNu == 42 || nNu == 43 || nNu == 45 || nNu == 46 || nNu == 48)
                {
                    nX = 49;     //기타 49
                }

                if (strBun == "29" || strBun == "31" || strBun == "32" || strBun == "33" || strBun == "36" || strBun == "39")
                {
                    nX = 13;        //치료재료대 13
                }
                #endregion

                #region //RPG.Amt1~7[x], R2Amt[x, y]
                if (strSuS == "2" || strSuS == "3" ||  strSuS == "4" || strSuS == "5" || strSuS == "6" || strSuS == "7" || strSuS == "8" || strSuS == "9")
                {
                    clsPmpaType.RPG.Amt1[24] = clsPmpaType.RPG.Amt1[24] + nAmt1;       //총액
                    clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;

                    clsPmpaType.RPG.Amt6[24] = clsPmpaType.RPG.Amt6[24] + n100JAmt;    //조합
                    //clsPmpaType.RPG.Amt6[50] = clsPmpaType.RPG.Amt6[50] + n100JAmt;

                    clsPmpaType.RPG.Amt5[24] = clsPmpaType.RPG.Amt5[24] + n100BAmt;    //본인
                    //clsPmpaType.RPG.Amt5[50] = clsPmpaType.RPG.Amt5[50] + n100BAmt;
                }
                else
                {
                    if (nSelf == 0)
                    {
                        clsPmpaType.RPG.Amt1[nX] = clsPmpaType.RPG.Amt1[nX] + nAmt1;
                        clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                    }
                    else if (nSelf == 1)
                    {
                        if (nX == 19 && clsPmpaType.TIT.Bi == "52")
                        {
                            clsPmpaType.RPG.Amt1[nX] = clsPmpaType.RPG.Amt1[nX] + nAmt1;
                            clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                        }
                        else
                        {
                            clsPmpaType.RPG.Amt2[nX] = clsPmpaType.RPG.Amt2[nX] + nAmt1;
                            clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                        }
                    }
                    else
                    {
                        clsPmpaType.RPG.Amt4[nX] = clsPmpaType.RPG.Amt4[nX] + nAmt1;
                        clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                    }
                    clsPmpaType.RPG.Amt3[nX] = clsPmpaType.RPG.Amt3[nX] + nAmt2;
                    clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                }

                if (nY != 0)
                {
                    clsPmpaPb.R2Amt[nY, nSelf] = clsPmpaPb.R2Amt[nY, nSelf] + nAmt1;
                    clsPmpaPb.R2Amt[nY, nSelf] = clsPmpaPb.R2Amt[nY, nSelf] + nAmt2;
                }

                clsPmpaPb.R2Amt[nNu, nSelf] = clsPmpaPb.R2Amt[nNu, nSelf] + nAmt1;
                clsPmpaPb.R2Amt[nNu, nSelf] = clsPmpaPb.R2Amt[nNu, nSelf] + nAmt2;

                clsPmpaPb.R2Amt[100, nSelf] = clsPmpaPb.R2Amt[100, nSelf] + nAmt1;
                clsPmpaPb.R2Amt[100, nSelf] = clsPmpaPb.R2Amt[100, nSelf] + nAmt2;

                clsPmpaPb.R2Amt[100, 6] = clsPmpaPb.R2Amt[100, 6] + nAmt1 + nAmt2;   //전체합
                #endregion

            }
           
            Dt.Dispose();
            Dt = null;

        }

        /// <summary>
        /// 연말정산용으로 별도로 만듬
        /// IPD_TRANS_PRTAmt_READ
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgTRSNO"></param>
        /// <param name="strTemp"></param>
        /// <seealso cref="Report_Print2.bas : IPD_TRANS_PRTAmt_READ "/>
        public void Ipd_Trans_PrtAmt_Read_Junsan(PsmhDb pDbCon, long ArgTRSNO, string ArgTempTrans)
        {
            int i = 0;
            DataTable Dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int J = 0;
            int nNu = 0;
            int nSelf = 0;
            string strBun = "";
            string strSuS = "";
            long nAmt1 = 0;
            long nAmt2 = 0;
            long n100BAmt = 0;     //'2014-03-26
            long n100JAmt = 0;     //'2014-03-26

            #region Variable Clear
            for (i = 1; i <= 100; i++)
            {
                for (J = 0; J < 7; J++)
                {
                    clsPmpaPb.R2Amt[i, J] = 0;
                }
            }

            clsPmpaType.RPG.Amt1 = new long[51];
            clsPmpaType.RPG.Amt2 = new long[51];
            clsPmpaType.RPG.Amt3 = new long[51];
            clsPmpaType.RPG.Amt4 = new long[51];
            clsPmpaType.RPG.Amt5 = new long[51];
            clsPmpaType.RPG.Amt6 = new long[51];
            clsPmpaType.RPG.Amt7 = new long[51];
            clsPmpaType.RPG.Amt8 = new long[51];
            clsPmpaType.RPG.Amt9 = new long[51];

            //항목별 합
            for (i = 0; i < 51; i++)
            {
                clsPmpaType.RPG.Amt1[i] = 0;  //급여합
                clsPmpaType.RPG.Amt2[i] = 0;  //비급여합
                clsPmpaType.RPG.Amt3[i] = 0;  //특진합
                clsPmpaType.RPG.Amt4[i] = 0;  //본인총액합
                clsPmpaType.RPG.Amt5[i] = 0;  //본인
                clsPmpaType.RPG.Amt6[i] = 0;  //공단
                clsPmpaType.RPG.Amt7[i] = 0;  //선별급여 총액
                clsPmpaType.RPG.Amt8[i] = 0;  //선별급여 조합
                clsPmpaType.RPG.Amt9[i] = 0;  //선별급여 본인
            }
            #endregion

            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.NU,a.Bun,a.GbSelf,b.SugbS GbSugbS,SUM(a.Amt1) Amt,SUM(a.Amt2) Amt2     ";
            SQL += ComNum.VBLF + "   FROM IPD_NEW_SLIP A, BAS_SUN b                                          ";
            SQL += ComNum.VBLF + "  WHERE a.Sunext=b.Sunext(+)                                               ";
            SQL += ComNum.VBLF + "   AND a.TRSNO = " + ArgTRSNO + "                                          ";
            if (ArgTempTrans == "임시자격") //2012-09-07
            {
                SQL += ComNum.VBLF + "   AND a.BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                SQL += ComNum.VBLF + "   AND a.BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
            }
            SQL += ComNum.VBLF + "  GROUP BY a.Nu,a.Bun,a.GbSelf,b.SugbS                                     ";

            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            for (i = 0; i < Dt.Rows.Count; i++)
            {
                nNu = Convert.ToInt16(Dt.Rows[i]["NU"].ToString());
                strBun = Dt.Rows[i]["Bun"].ToString().Trim();
                strSuS = Dt.Rows[i]["GbSugbS"].ToString().Trim();  //100/100

                nAmt1 = Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString()));      //금액1
                nAmt2 = Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt2"].ToString()));     //특진

                if (strSuS == "4")
                {
                    n100BAmt = (long)Math.Round((nAmt1 * 0.8), 0, MidpointRounding.AwayFromZero);
                    n100JAmt = nAmt1 - (long)Math.Round((nAmt1 * 0.8), 0, MidpointRounding.AwayFromZero);
                }
                else if (strSuS == "5")
                {
                    n100BAmt = (long)Math.Round((nAmt1 * 0.5), 0, MidpointRounding.AwayFromZero);
                    n100JAmt = nAmt1 - (long)Math.Round((nAmt1 * 0.5), 0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    n100BAmt = 0;
                    n100JAmt = 0;
                }

                switch (Dt.Rows[i]["GbSelf"].ToString().Trim())
                {
                    case "0":
                        nSelf = 0;
                        if (strSuS == "3" ||  strSuS == "4" || strSuS == "5"||strSuS == "6" || strSuS == "7" || strSuS == "8" || strSuS == "9" )
                            nSelf = 2;
                        break;
                    case "1":
                        nSelf = 1;
                        break;
                    case "2":
                        nSelf = 2;
                        if (strSuS == "1")
                            nSelf = 2;
                        else
                            nSelf = 1;
                        break;
                    default:
                        nSelf = 0;
                        break;
                }

                switch (strBun)
                {
                    //"29", "31", "32", "33", "36", "39"  '치료재료대 13
                    case "29":
                    case "31":
                    case "32":
                    case "33":
                    case "36":
                    case "39":
                        if (strSuS == "3" || strSuS == "4" || strSuS == "5" || strSuS == "6" || strSuS == "7" || strSuS == "8" || strSuS == "9" )
                        {
                            clsPmpaType.RPG.Amt4[13] = clsPmpaType.RPG.Amt4[13] + n100BAmt; //'본인부담
                            clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + n100BAmt; //'본인부담
                            clsPmpaType.RPG.Amt8[13] = clsPmpaType.RPG.Amt8[13] + n100JAmt; //'조합부담
                            clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + n100JAmt;
                        }
                        else
                        {
                            if (nSelf == 0)
                            {
                                clsPmpaType.RPG.Amt1[13] = clsPmpaType.RPG.Amt1[13] + nAmt1;
                                clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                            }
                            else if (nSelf == 1)
                            {
                                clsPmpaType.RPG.Amt2[13] = clsPmpaType.RPG.Amt2[13] + nAmt1;
                                clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                            }
                            else
                            {
                                clsPmpaType.RPG.Amt4[13] = clsPmpaType.RPG.Amt4[13] + nAmt1;
                                clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                            }
                        }
                        break;
                    default:
                        switch (nNu)
                        {
                            case 1: //진찰료 1
                                if (nSelf == 0)
                                {
                                    clsPmpaType.RPG.Amt1[1] = clsPmpaType.RPG.Amt1[1] + nAmt1;
                                    clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                }
                                else if (nSelf == 1)
                                {
                                    clsPmpaType.RPG.Amt2[1] = clsPmpaType.RPG.Amt2[1] + nAmt1;
                                    clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[1] = clsPmpaType.RPG.Amt4[1] + nAmt1;
                                    clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                }
                                clsPmpaType.RPG.Amt3[1] = clsPmpaType.RPG.Amt3[1] + nAmt2;
                                clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                                break;
                            case 2:
                            case 3:
                            case 21:
                                //입원료
                                if (nSelf == 0)
                                {
                                    clsPmpaType.RPG.Amt1[2] = clsPmpaType.RPG.Amt1[2] + nAmt1;
                                    clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                }
                                else if (nSelf == 1)
                                {
                                    clsPmpaType.RPG.Amt2[2] = clsPmpaType.RPG.Amt2[2] + nAmt1;
                                    clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[2] = clsPmpaType.RPG.Amt4[2] + nAmt1;
                                    clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                }
                                clsPmpaType.RPG.Amt3[2] = clsPmpaType.RPG.Amt3[2] + nAmt2;
                                clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;

                                break;
                            case 16:
                            case 34:
                                //식대
                                if (nSelf == 0)
                                {
                                    clsPmpaType.RPG.Amt1[3] = clsPmpaType.RPG.Amt1[3] + nAmt1;
                                    clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                }
                                else if (nSelf == 1)
                                {
                                    clsPmpaType.RPG.Amt2[3] = clsPmpaType.RPG.Amt2[3] + nAmt1;
                                    clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[3] = clsPmpaType.RPG.Amt4[3] + nAmt1;
                                    clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                }
                                clsPmpaType.RPG.Amt3[3] = clsPmpaType.RPG.Amt3[3] + nAmt2;
                                clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                                break;
                            case 4:
                            case 22:
                                //투약
                                switch (strBun)
                                {
                                    case "11":
                                    case "12":
                                        if (nSelf == 0)
                                        {
                                            clsPmpaType.RPG.Amt1[5] = clsPmpaType.RPG.Amt1[5] + nAmt1;
                                            clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                        }
                                        else if (nSelf == 1)
                                        {
                                            clsPmpaType.RPG.Amt2[5] = clsPmpaType.RPG.Amt2[5] + nAmt1;
                                            clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                        }
                                        else
                                        {
                                            clsPmpaType.RPG.Amt4[5] = clsPmpaType.RPG.Amt4[5] + nAmt1;
                                            clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                        }
                                        clsPmpaType.RPG.Amt3[5] = clsPmpaType.RPG.Amt3[5] + nAmt2;
                                        clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;

                                        clsPmpaPb.R2Amt[71, nSelf] = clsPmpaPb.R2Amt[71, nSelf] + nAmt1;
                                        clsPmpaPb.R2Amt[71, nSelf] = clsPmpaPb.R2Amt[71, nSelf] + nAmt2;
                                        break;
                                    default:
                                        if (nSelf == 0)
                                        {
                                            clsPmpaType.RPG.Amt1[4] = clsPmpaType.RPG.Amt1[4] + nAmt1;
                                            clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                        }
                                        else if (nSelf == 1)
                                        {
                                            clsPmpaType.RPG.Amt2[4] = clsPmpaType.RPG.Amt2[4] + nAmt1;
                                            clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                        }
                                        else
                                        {
                                            clsPmpaType.RPG.Amt4[4] = clsPmpaType.RPG.Amt4[4] + nAmt1;
                                            clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                        }
                                        clsPmpaType.RPG.Amt3[4] = clsPmpaType.RPG.Amt3[4] + nAmt2;
                                        clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;

                                        clsPmpaPb.R2Amt[70, nSelf] = clsPmpaPb.R2Amt[70, nSelf] + nAmt1;
                                        clsPmpaPb.R2Amt[70, nSelf] = clsPmpaPb.R2Amt[70, nSelf] + nAmt2;
                                        break;
                                }
                                break;
                            case 5:
                            case 23:
                                //주사
                                switch (strBun)
                                {
                                    case "20":
                                    case "21":
                                        if (nSelf == 0)
                                        {
                                            clsPmpaType.RPG.Amt1[7] = clsPmpaType.RPG.Amt1[7] + nAmt1;
                                            clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                        }
                                        else if (nSelf == 1)
                                        {
                                            clsPmpaType.RPG.Amt2[7] = clsPmpaType.RPG.Amt2[7] + nAmt1;
                                            clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                        }
                                        else
                                        {
                                            clsPmpaType.RPG.Amt4[7] = clsPmpaType.RPG.Amt4[7] + nAmt1;
                                            clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                        }
                                        clsPmpaType.RPG.Amt3[7] = clsPmpaType.RPG.Amt3[7] + nAmt2;
                                        clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;

                                        clsPmpaPb.R2Amt[73, nSelf] = clsPmpaPb.R2Amt[73, nSelf] + nAmt1;
                                        clsPmpaPb.R2Amt[73, nSelf] = clsPmpaPb.R2Amt[73, nSelf] + nAmt2;
                                        break;
                                    default:
                                        if (nSelf == 0)
                                        {
                                            clsPmpaType.RPG.Amt1[6] = clsPmpaType.RPG.Amt1[6] + nAmt1;
                                            clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                        }
                                        else if (nSelf == 1)
                                        {
                                            clsPmpaType.RPG.Amt2[6] = clsPmpaType.RPG.Amt2[6] + nAmt1;
                                            clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                        }
                                        else
                                        {
                                            clsPmpaType.RPG.Amt4[6] = clsPmpaType.RPG.Amt4[6] + nAmt1;
                                            clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                        }
                                        clsPmpaType.RPG.Amt3[6] = clsPmpaType.RPG.Amt3[6] + nAmt2;
                                        clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;

                                        clsPmpaPb.R2Amt[72, nSelf] = clsPmpaPb.R2Amt[72, nSelf] + nAmt1;
                                        clsPmpaPb.R2Amt[72, nSelf] = clsPmpaPb.R2Amt[72, nSelf] + nAmt2;
                                        break;
                                }
                                break;
                            case 6:
                            case 24:
                                //마취료
                                if (nSelf == 0)
                                {
                                    clsPmpaType.RPG.Amt1[8] = clsPmpaType.RPG.Amt1[8] + nAmt1;
                                    clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                }
                                else if (nSelf == 1)
                                {
                                    clsPmpaType.RPG.Amt2[8] = clsPmpaType.RPG.Amt2[8] + nAmt1;
                                    clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[8] = clsPmpaType.RPG.Amt4[8] + nAmt1;
                                    clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                }
                                clsPmpaType.RPG.Amt3[8] = clsPmpaType.RPG.Amt3[8] + nAmt2;
                                clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                                break;
                            case 9:
                            case 10:
                            case 12:
                            case 27:
                            case 28:
                            case 30:
                                //처치
                                if (nSelf == 0)
                                {
                                    clsPmpaType.RPG.Amt1[9] = clsPmpaType.RPG.Amt1[9] + nAmt1;
                                    clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                }
                                else if (nSelf == 1)
                                {
                                    clsPmpaType.RPG.Amt2[9] = clsPmpaType.RPG.Amt2[9] + nAmt1;
                                    clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[9] = clsPmpaType.RPG.Amt4[9] + nAmt1;
                                    clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                }
                                clsPmpaType.RPG.Amt3[9] = clsPmpaType.RPG.Amt3[9] + nAmt2;
                                clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                                break;
                            case 13:
                            case 14:
                            case 31:
                            case 32:
                                //검사
                                if (nSelf == 0)
                                {
                                    clsPmpaType.RPG.Amt1[10] = clsPmpaType.RPG.Amt1[10] + nAmt1;
                                    clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                }
                                else if (nSelf == 1)
                                {
                                    clsPmpaType.RPG.Amt2[10] = clsPmpaType.RPG.Amt2[10] + nAmt1;
                                    clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[10] = clsPmpaType.RPG.Amt4[10] + nAmt1;
                                    clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                }
                                clsPmpaType.RPG.Amt3[10] = clsPmpaType.RPG.Amt3[10] + nAmt2;
                                clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                                break;
                            case 15:
                            case 33:
                                //영상진단
                                if (nSelf == 0)
                                {
                                    clsPmpaType.RPG.Amt1[11] = clsPmpaType.RPG.Amt1[11] + nAmt1;
                                    clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                }
                                else if (nSelf == 1)
                                {
                                    clsPmpaType.RPG.Amt2[11] = clsPmpaType.RPG.Amt2[11] + nAmt1;
                                    clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[11] = clsPmpaType.RPG.Amt4[11] + nAmt1;
                                    clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                }
                                clsPmpaType.RPG.Amt3[11] = clsPmpaType.RPG.Amt3[11] + nAmt2;
                                clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;

                                switch (strBun)
                                {
                                    case "65":
                                        clsPmpaPb.R2Amt[74, nSelf] = clsPmpaPb.R2Amt[74, nSelf] + nAmt1;
                                        clsPmpaPb.R2Amt[74, nSelf] = clsPmpaPb.R2Amt[74, nSelf] + nAmt2;
                                        break;
                                    default:
                                        clsPmpaPb.R2Amt[75, nSelf] = clsPmpaPb.R2Amt[75, nSelf] + nAmt1;
                                        clsPmpaPb.R2Amt[75, nSelf] = clsPmpaPb.R2Amt[75, nSelf] + nAmt2;
                                        break;
                                }
                                break;
                            case 7:
                            case 25:
                                //물리치료
                                if (nSelf == 0)
                                {
                                    clsPmpaType.RPG.Amt1[14] = clsPmpaType.RPG.Amt1[14] + nAmt1;
                                    clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                }
                                else if (nSelf == 1)
                                {
                                    clsPmpaType.RPG.Amt2[14] = clsPmpaType.RPG.Amt2[14] + nAmt1;
                                    clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[14] = clsPmpaType.RPG.Amt4[14] + nAmt1;
                                    clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                }
                                clsPmpaType.RPG.Amt3[14] = clsPmpaType.RPG.Amt3[14] + nAmt2;
                                clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                                break;
                            case 8:
                            case 26:
                                //정신요법
                                if (nSelf == 0)
                                {
                                    clsPmpaType.RPG.Amt1[15] = clsPmpaType.RPG.Amt1[15] + nAmt1;
                                    clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                }
                                else if (nSelf == 1)
                                {
                                    clsPmpaType.RPG.Amt2[15] = clsPmpaType.RPG.Amt2[15] + nAmt1;
                                    clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[15] = clsPmpaType.RPG.Amt4[15] + nAmt1;
                                    clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                }
                                clsPmpaType.RPG.Amt3[15] = clsPmpaType.RPG.Amt3[15] + nAmt2;
                                clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                                break;
                            case 11:
                            case 29:
                                //수혈료
                                if (nSelf == 0)
                                {
                                    clsPmpaType.RPG.Amt1[16] = clsPmpaType.RPG.Amt1[16] + nAmt1;
                                    clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                }
                                else if (nSelf == 1)
                                {
                                    clsPmpaType.RPG.Amt2[16] = clsPmpaType.RPG.Amt2[16] + nAmt1;
                                    clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[16] = clsPmpaType.RPG.Amt4[16] + nAmt1;
                                    clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                }
                                clsPmpaType.RPG.Amt3[16] = clsPmpaType.RPG.Amt3[16] + nAmt2;
                                clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                                break;
                            case 19:
                            case 37:
                                //CT
                                if (nSelf == 0)
                                {
                                    clsPmpaType.RPG.Amt1[17] = clsPmpaType.RPG.Amt1[17] + nAmt1;
                                    clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                }
                                else if (nSelf == 1)
                                {
                                    clsPmpaType.RPG.Amt2[17] = clsPmpaType.RPG.Amt2[17] + nAmt1;
                                    clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[17] = clsPmpaType.RPG.Amt4[17] + nAmt1;
                                    clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                }
                                clsPmpaType.RPG.Amt3[17] = clsPmpaType.RPG.Amt3[17] + nAmt2;
                                clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                                break;
                            case 18:
                            case 38:
                                //MRI
                                if (nSelf == 0)
                                {
                                    clsPmpaType.RPG.Amt1[18] = clsPmpaType.RPG.Amt1[18] + nAmt1;
                                    clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                }
                                else if (nSelf == 1)
                                {
                                    clsPmpaType.RPG.Amt2[18] = clsPmpaType.RPG.Amt2[18] + nAmt1;
                                    clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[18] = clsPmpaType.RPG.Amt4[18] + nAmt1;
                                    clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                }
                                clsPmpaType.RPG.Amt3[18] = clsPmpaType.RPG.Amt3[18] + nAmt2;
                                clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                                break;
                            case 36:
                                //초음파
                                if (clsPmpaType.TIT.Bi == "52") //52 비급->급여
                                {
                                    if (nSelf == 0)
                                    {
                                        clsPmpaType.RPG.Amt1[19] = clsPmpaType.RPG.Amt1[19] + nAmt1;
                                        clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                    }
                                    else if (nSelf == 1)
                                    {
                                        clsPmpaType.RPG.Amt1[19] = clsPmpaType.RPG.Amt1[19] + nAmt1;
                                        clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                    }
                                    else
                                    {
                                        clsPmpaType.RPG.Amt1[19] = clsPmpaType.RPG.Amt1[19] + nAmt1;
                                        clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                    }
                                    clsPmpaType.RPG.Amt3[19] = clsPmpaType.RPG.Amt3[19] + nAmt2;
                                    clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                                }
                                else
                                {
                                    if (nSelf == 0)
                                    {
                                        clsPmpaType.RPG.Amt1[19] = clsPmpaType.RPG.Amt1[19] + nAmt1;
                                        clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                    }
                                    else if (nSelf == 1)
                                    {
                                        clsPmpaType.RPG.Amt2[19] = clsPmpaType.RPG.Amt2[19] + nAmt1;
                                        clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                    }
                                    else
                                    {
                                        clsPmpaType.RPG.Amt4[19] = clsPmpaType.RPG.Amt4[19] + nAmt1;
                                        clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                    }
                                    clsPmpaType.RPG.Amt3[19] = clsPmpaType.RPG.Amt3[19] + nAmt2;
                                    clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                                }
                                break;
                            case 40:
                                //보철교정
                                if (nSelf == 0)
                                {
                                    clsPmpaType.RPG.Amt1[20] = clsPmpaType.RPG.Amt1[20] + nAmt1;
                                    clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                }
                                else if (nSelf == 1)
                                {
                                    clsPmpaType.RPG.Amt2[20] = clsPmpaType.RPG.Amt2[20] + nAmt1;
                                    clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[20] = clsPmpaType.RPG.Amt4[20] + nAmt1;
                                    clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                }
                                clsPmpaType.RPG.Amt3[20] = clsPmpaType.RPG.Amt3[20] + nAmt2;
                                clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                                break;
                            case 35:
                                //병실차액
                                if (nSelf == 0)
                                {
                                    clsPmpaType.RPG.Amt1[21] = clsPmpaType.RPG.Amt1[21] + nAmt1;
                                    clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                }
                                else if (nSelf == 1)
                                {
                                    clsPmpaType.RPG.Amt2[21] = clsPmpaType.RPG.Amt2[21] + nAmt1;
                                    clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[21] = clsPmpaType.RPG.Amt4[21] + nAmt1;
                                    clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                }
                                clsPmpaType.RPG.Amt3[21] = clsPmpaType.RPG.Amt3[21] + nAmt2;
                                clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                                break;
                            case 47:
                                //증명료
                                if (nSelf == 0)
                                {
                                    clsPmpaType.RPG.Amt1[22] = clsPmpaType.RPG.Amt1[22] + nAmt1;
                                    clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                }
                                else if (nSelf == 1)
                                {
                                    clsPmpaType.RPG.Amt2[22] = clsPmpaType.RPG.Amt2[22] + nAmt1;
                                    clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[22] = clsPmpaType.RPG.Amt4[22] + nAmt1;
                                    clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                }
                                clsPmpaType.RPG.Amt3[22] = clsPmpaType.RPG.Amt3[22] + nAmt2;
                                clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                                break;
                            case 17:
                            case 20:
                            case 39:
                            case 41:
                            case 42:
                            case 43:
                            case 45:
                            case 46:
                                //기타
                                if (nSelf == 0)
                                {
                                    clsPmpaType.RPG.Amt1[49] = clsPmpaType.RPG.Amt1[49] + nAmt1;
                                    clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                                }
                                else if (nSelf == 1)
                                {
                                    clsPmpaType.RPG.Amt2[49] = clsPmpaType.RPG.Amt2[49] + nAmt1;
                                    clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[49] = clsPmpaType.RPG.Amt4[49] + nAmt1;
                                    clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                                }
                                clsPmpaType.RPG.Amt3[49] = clsPmpaType.RPG.Amt3[49] + nAmt2;
                                clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                                break;
                        }
                        break;
                }

                clsPmpaPb.R2Amt[nNu, nSelf] = clsPmpaPb.R2Amt[nNu, nSelf] + nAmt1;
                clsPmpaPb.R2Amt[nNu, nSelf] = clsPmpaPb.R2Amt[nNu, nSelf] + nAmt2;
                clsPmpaPb.R2Amt[100, nSelf] = clsPmpaPb.R2Amt[100, nSelf] + nAmt1;
                clsPmpaPb.R2Amt[100, nSelf] = clsPmpaPb.R2Amt[100, nSelf] + nAmt2;
                clsPmpaPb.R2Amt[100, 6] = clsPmpaPb.R2Amt[100, 6] + nAmt1 + nAmt2;// '전체합

            }
            Dt.Dispose();
            Dt = null;
        }


        /// <summary>
        /// 연말정산용으로 별도로 만듬
        /// Ipd_Trans_PrtAmt_Read_New
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgTRSNO"></param>
        /// <param name="strTemp"></param>
        /// <seealso cref="Report_Print2.bas : Ipd_Trans_PrtAmt_Read_New "/>
        public bool Ipd_Trans_PrtAmt_Read_New_Junsan(PsmhDb pDbCon, long ArgTRSNO, string ArgTempTrans)
        {
            int i = 0;
            int j = 0;
            int nNu = 0;
            int nSelf = 0;
            int nX = 0;
            int nY = 0;
            long nAmt1 = 0;
            long nAmt2 = 0;
            long n100BAmt = 0;
            long n100JAmt = 0;
            string strBun = "";
            string strSuS = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //slip clear
            for (i = 1; i <= 100; i++)
            {
                for (j = 0; j <= 6; j++)
                {
                    clsPmpaPb.R2Amt[i, j] = 0;
                }
            }

            //항목별 합
            for (i = 0; i <= 50; i++)
            {
                clsPmpaType.RPG.Amt1[i] = 0; //급여합
                clsPmpaType.RPG.Amt2[i] = 0; //비급여합
                clsPmpaType.RPG.Amt3[i] = 0; //특진합
                clsPmpaType.RPG.Amt4[i] = 0; //본인총액합
                clsPmpaType.RPG.Amt5[i] = 0; //본인
                clsPmpaType.RPG.Amt6[i] = 0; //공단
                clsPmpaType.RPG.Amt7[i] = 0; //선별급여 총액
                clsPmpaType.RPG.Amt8[i] = 0; //선별급여 조합
                clsPmpaType.RPG.Amt9[i] = 0; //선별급여 본인
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT NU,Bun,GbSelf,GbSugbS,SUM(Amt1) Amt,SUM(Amt2) Amt2    ";
                SQL = SQL + ComNum.VBLF + "   FROM IPD_NEW_SLIP                                          ";
                SQL = SQL + ComNum.VBLF + "  WHERE TRSNO = " + ArgTRSNO + "                              ";

                if (ArgTempTrans == "임시자격")
                {
                    SQL = SQL + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')  ";
                    SQL = SQL + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD') ";
                }
                SQL = SQL + ComNum.VBLF + "  GROUP BY Nu,Bun,GbSelf,GbSugbS                                ";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nNu = (int)VB.Val(dt.Rows[i]["NU"].ToString().Trim());
                        strBun = dt.Rows[i]["BUN"].ToString().Trim();
                        strSuS = dt.Rows[i]["GBSUGBS"].ToString().Trim(); //100/100

                        nAmt1 = (int)VB.Val(dt.Rows[i]["AMT"].ToString().Trim()); //금액1
                        nAmt2 = (int)VB.Val(dt.Rows[i]["AMT2"].ToString().Trim()); //특진

                        //2014-03-26
                        if (strSuS == "4" || strSuS == "6")
                        {
                            n100BAmt = (long)Math.Round(((double)nAmt1 * 80 / 100));
                            n100JAmt = nAmt1 - (long)Math.Round(((double)nAmt1 * 80 / 100));
                        }
                        else if (strSuS == "3" )
                        {
                            n100BAmt = (long)Math.Round(((double)nAmt1 * 30 / 100));
                            n100JAmt = nAmt1 - ((long)Math.Round(((double)nAmt1 * 30 / 100)));
                        }
                        else if (strSuS == "5" || strSuS == "7")
                        {
                            n100BAmt = (long)Math.Round(((double)nAmt1 * 50 / 100));
                            n100JAmt = nAmt1 - ((long)Math.Round(((double)nAmt1 * 50 / 100)));
                        }
                        else if (strSuS == "8" || strSuS == "9")
                        {
                            n100BAmt = (long)Math.Round(((double)nAmt1 * 90 / 100)); //2018-01-01 add SugbS(ArgCnt) = "8" SugbS
                            n100JAmt = nAmt1 - ((long)Math.Round(((double)nAmt1 * 90 / 100)));
                        }
                        else
                        {
                            n100BAmt = 0;
                            n100JAmt = 0;
                        }

                        //if (strBun == "23")
                        //{
                        //    i = i;
                        //}

                        if (string.Compare(clsPmpaPb.GstrSysDate, "2012-01-10") >= 0)
                        {
                            switch (dt.Rows[i]["GBSELF"].ToString().Trim())
                            {
                                case "0":
                                    nSelf = 0;
                                    //2014-03-26
                                    if (strSuS == "3" ||  strSuS == "4" || strSuS == "5" || strSuS == "6" || strSuS == "7" || strSuS == "8" || strSuS == "9") //2018-01-01 add Sugbs(ArgCnt) = "8" SugbS
                                    {
                                        nSelf = 2;
                                    }

                                    break;
                                case "1":
                                    nSelf = 1;
                                    break;
                                case "2":
                                    nSelf = 2;

                                    if (strSuS == "1")
                                    {
                                        nSelf = 2;
                                    }
                                    else
                                    {
                                        nSelf = 1;
                                    }

                                    break;
                                default:
                                    nSelf = 0;
                                    break;
                            }
                        }
                        else
                        {
                            switch (dt.Rows[i]["GBSELF"].ToString().Trim())
                            {
                                case "0":
                                    nSelf = 0;
                                    break;
                                case "1":
                                    nSelf = 1;
                                    break;
                                case "2":
                                    nSelf = 2;
                                    break;
                                default:
                                    nSelf = 0;
                                    break;
                            }
                        }

                        nY = 0;

                        //변수배열 분류
                        switch (nNu)
                        {
                            case 1:
                                nX = 1; //진찰료
                                break;
                            case 2:
                            case 3:
                            case 21:
                                nX = 2; //입원료 2
                                break;
                            case 16:
                            case 34:
                                nX = 3; //식대3
                                break;
                            case 4:
                            case 22:
                                nX = (strBun == "11" || strBun == "12") ? 5 : 4; //투약 4
                                nY = (strBun == "11" || strBun == "12") ? 71 : 70; //투약4
                                break;
                            case 5:
                            case 23:
                                nX = (strBun == "20" || strBun == "21") ? 7 : 6; //투약 4
                                nY = (strBun == "20" || strBun == "21") ? 73 : 72; //투약4
                                break;
                            case 6:
                            case 24:
                                nX = 8; //마취료 8
                                break;
                            case 9:
                            case 10:
                            case 12:
                            case 27:
                            case 28:
                            case 30:
                                nX = 9; //처치 9
                                break;
                            case 13:
                            case 14:
                            case 31:
                            case 32:
                                nX = 10; //검사 10
                                break;
                            case 15:
                            case 33:
                                nX = 11; //영상진단 11
                                nY = strBun == "65" ? 74 : 75; //영상진단 11
                                break;
                            case 7:
                            case 25:
                                nX = 14; //물리치료 14
                                break;
                            case 8:
                            case 26:
                                nX = 15; //정신요법 15
                                break;
                            case 11:
                            case 29:
                                nX = 16; //수혈료 16
                                break;
                            case 19:
                            case 37:
                                nX = 17; //CT 17
                                break;
                            case 18:
                            case 38:
                                nX = 18; //MRI 18
                                break;
                            case 36:
                                nX = 19; //초음파 19
                                break;
                            case 40:
                                nX = 20; //보철교정 20
                                break;
                            case 35:
                                nX = 21; //병실차액 21
                                break;
                            case 47:
                                nX = 22; //증명료 22
                                break;
                            case 17:
                            case 20:
                            case 39:
                            case 41:
                            case 42:
                            case 43:
                            case 45:
                            case 46:
                            case 48: //기타 49
                                nX = 49;
                                break;
                        }

                        switch (strBun)
                        {
                            case "29":
                            case "31":
                            case "32":
                            case "33":
                            case "36":
                            case "39": //치료재료대 13
                                nX = 13;
                                break;
                        }

                        if (nX == 3)
                        {
                            nX = nX;
                        }

                        //2014-03-26
                        if (strSuS == "3" ||  strSuS == "4" || strSuS == "5" || strSuS == "6" || strSuS == "7" || strSuS == "8" || strSuS == "9")
                        {
                            clsPmpaType.RPG.Amt7[nX] = clsPmpaType.RPG.Amt7[nX] + nAmt1; //총액
                            clsPmpaType.RPG.Amt7[50] = clsPmpaType.RPG.Amt7[50] + nAmt1;

                            clsPmpaType.RPG.Amt8[nX] = clsPmpaType.RPG.Amt8[nX] + n100JAmt; //조합
                            clsPmpaType.RPG.Amt8[50] = clsPmpaType.RPG.Amt8[50] + n100JAmt;

                            clsPmpaType.RPG.Amt9[nX] = clsPmpaType.RPG.Amt9[nX] + n100BAmt; //본인
                            clsPmpaType.RPG.Amt9[50] = clsPmpaType.RPG.Amt9[50] + n100BAmt;
                        }
                        else
                        {
                            if (nSelf == 0)
                            {
                                clsPmpaType.RPG.Amt1[nX] = clsPmpaType.RPG.Amt1[nX] + nAmt1;
                                clsPmpaType.RPG.Amt1[50] = clsPmpaType.RPG.Amt1[50] + nAmt1;
                            }
                            else if (nSelf == 1)
                            {
                                clsPmpaType.RPG.Amt2[nX] = clsPmpaType.RPG.Amt2[nX] + nAmt1;
                                clsPmpaType.RPG.Amt2[50] = clsPmpaType.RPG.Amt2[50] + nAmt1;
                            }
                            else
                            {
                                clsPmpaType.RPG.Amt4[nX] = clsPmpaType.RPG.Amt4[nX] + nAmt1;
                                clsPmpaType.RPG.Amt4[50] = clsPmpaType.RPG.Amt4[50] + nAmt1;
                            }

                            clsPmpaType.RPG.Amt3[nX] = clsPmpaType.RPG.Amt3[nX] + nAmt2;
                            clsPmpaType.RPG.Amt3[50] = clsPmpaType.RPG.Amt3[50] + nAmt2;
                        }

                        if (nY != 0)
                        {
                            clsPmpaPb.R2Amt[nY, nSelf] = clsPmpaPb.R2Amt[nY, nSelf] + nAmt1;
                            clsPmpaPb.R2Amt[nY, nSelf] = clsPmpaPb.R2Amt[nY, nSelf] + nAmt2;
                        }

                        clsPmpaPb.R2Amt[nNu, nSelf] = clsPmpaPb.R2Amt[nNu, nSelf] + nAmt1;
                        clsPmpaPb.R2Amt[nNu, nSelf] = clsPmpaPb.R2Amt[nNu, nSelf] + nAmt2;

                        clsPmpaPb.R2Amt[100, nSelf] = clsPmpaPb.R2Amt[100, nSelf] + nAmt1;
                        clsPmpaPb.R2Amt[100, nSelf] = clsPmpaPb.R2Amt[100, nSelf] + nAmt2;

                        clsPmpaPb.R2Amt[100, 6] = clsPmpaPb.R2Amt[100, 6] + nAmt2; //전체합
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }
        }

        /// <summary>
        /// Description : IPD_NEW_SLIP 읽어서 계산용 변수 세팅하기 RPG.AMT(비급여, 전액본인부담)
        /// Author : 김민철
        /// Create Date : 2018.01.29
        /// </summary>
        /// <param name="ArgTRSNO"></param>
        /// <seealso cref="Report_Print2.bas : IPD_TRANS_PRTAmt_READ_DRG "/>
        public void Ipd_Trans_PrtAmt_Read_Drg(PsmhDb pDbCon, long ArgTRSNO)
        {
            int i = 0, j = 0, nRead = 0;
            int nNu = 0;
            string strBun = "", str100 = "", strGf = "", strSelf = "";
            string strSugba = "";

            long nAmt1 = 0, nAmt2 = 0;

            DataTable Dt = new DataTable();
            DataTable Dt2 = new DataTable();
            clsPmpaPb.GnH1RoomAmt = 0; //1인실 비급여 금액 담아두기
            string SQL = "";
            string SqlErr = "";

            #region Variable Clear
            for (i = 1; i < 101; i++)
            {
                for (j = 0; j < 7; j++)
                {
                    clsPmpaPb.R2Amt[i, j] = 0;
                }
            }

            clsPmpaType.RPG.Amt1 = new long[51];
            clsPmpaType.RPG.Amt2 = new long[51];
            clsPmpaType.RPG.Amt3 = new long[51];
            clsPmpaType.RPG.Amt4 = new long[51];
            clsPmpaType.RPG.Amt5 = new long[51];
            clsPmpaType.RPG.Amt6 = new long[51];

            //항목별 합
            for (i = 0; i < 51; i++)
            {
                clsPmpaType.RPG.Amt1[i] = 0;  //급여합
                clsPmpaType.RPG.Amt2[i] = 0;  //비급여합
                clsPmpaType.RPG.Amt3[i] = 0;  //특진합
                clsPmpaType.RPG.Amt4[i] = 0;  //본인총액합
                clsPmpaType.RPG.Amt5[i] = 0;  //본인
                clsPmpaType.RPG.Amt6[i] = 0;  //공단
            }
            #endregion

            #region 분류별 금액 인쇄용 변수 RPG.Amt 에 저장
            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.NU, a.Bun, a.GbSelf, b.DRG100, b.DRGF, a.GbSelf,                 ";
            SQL += ComNum.VBLF + "        SUM(a.Amt1) Amt, SUM(a.Amt2) Amt2                                  ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a,                              ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b                                    ";
            SQL += ComNum.VBLF + "  WHERE a.TRSNO = " + ArgTRSNO + "                                         ";
            SQL += ComNum.VBLF + "    AND a.SUNEXT = b.SUNEXT(+)                                             ";
            SQL += ComNum.VBLF + "    AND a.SUNEXT NOT IN ('DRG001','DRG002')                                ";
            SQL += ComNum.VBLF + "  GROUP BY a.Nu, a.Bun, a.GbSelf, b.DRG100, b.DRGF, a.GbSelf               ";

            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            for (i = 0; i < Dt.Rows.Count; i++)
            {
                nNu = Convert.ToInt16(Dt.Rows[i]["NU"].ToString());
                strBun = Dt.Rows[i]["Bun"].ToString().Trim();

                str100 = Dt.Rows[i]["DRG100"].ToString().Trim();     //인정비급여
                strGf = Dt.Rows[i]["DRGF"].ToString().Trim();        //DRG비급여
                strSelf = Dt.Rows[i]["GbSelf"].ToString().Trim();

                nAmt1 = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());      //금액1
                nAmt2 = Convert.ToInt64(Dt.Rows[i]["Amt2"].ToString());     //특진

                #region RPG.Amt 변수에 금액세팅
                if (strBun == "29" || strBun == "31" || strBun == "32" || strBun == "33" || strBun == "36" || strBun == "39")
                {
                    if (str100 == "Y")         //인정비급여
                    {
                        clsPmpaType.RPG.Amt4[13] += nAmt1;
                        clsPmpaType.RPG.Amt4[50] += nAmt1;
                    }
                    else if (strGf == "Y")        //비급여
                    {
                        clsPmpaType.RPG.Amt2[13] += nAmt1;
                        clsPmpaType.RPG.Amt2[50] += nAmt1;
                    }

                    clsPmpaType.RPG.Amt3[13] += nAmt2;   //선택진료
                    clsPmpaType.RPG.Amt3[50] += nAmt2;
                }
                else
                {
                    if (nNu == 1)
                    {
                        #region 진찰료
                        if (str100 == "Y")
                        {
                            clsPmpaType.RPG.Amt4[1] += nAmt1;
                            clsPmpaType.RPG.Amt4[50] += nAmt1;
                        }
                        else if (strGf == "Y")
                        {
                            clsPmpaType.RPG.Amt2[1] += nAmt1;
                            clsPmpaType.RPG.Amt2[50] += nAmt1;
                        }
                        clsPmpaType.RPG.Amt3[1] += nAmt2;
                        clsPmpaType.RPG.Amt3[50] += nAmt2;
                        #endregion
                    }
                    else if (nNu == 2 || nNu == 3 || nNu == 21)
                    {
                        #region 입원료
                        if (str100 == "Y")
                        {
                            clsPmpaType.RPG.Amt4[2] += nAmt1;
                            clsPmpaType.RPG.Amt4[50] += nAmt1;
                           
                        }
                        else if (strGf == "Y")
                        {
                            clsPmpaType.RPG.Amt2[2] += nAmt1;
                            clsPmpaType.RPG.Amt2[50] += nAmt1;
                            if (nNu == 21) { clsPmpaPb.GnH1RoomAmt += nAmt1; } //1인실 비급여 금액 담아두기
                        }
                        clsPmpaType.RPG.Amt3[2] += nAmt2;
                        clsPmpaType.RPG.Amt3[50] += nAmt2;
                        
                        #endregion
                    }
                    else if (nNu == 4 || nNu == 22)
                    {
                        #region 투약료
                        if (strBun == "11" || strBun == "12")
                        {
                            if (str100 == "Y")
                            {
                                clsPmpaType.RPG.Amt4[5] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (strGf == "Y")
                            {
                                clsPmpaType.RPG.Amt2[5] += nAmt1;
                                clsPmpaType.RPG.Amt2[50] += nAmt1;
                            }
                            clsPmpaType.RPG.Amt3[5] += nAmt2;
                            clsPmpaType.RPG.Amt3[50] += nAmt2;
                        }
                        else
                        {
                            if (str100 == "Y")
                            {
                                clsPmpaType.RPG.Amt4[4] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (strGf == "Y")
                            {
                                clsPmpaType.RPG.Amt2[4] += nAmt1;
                                clsPmpaType.RPG.Amt2[50] += nAmt1;
                            }
                            clsPmpaType.RPG.Amt3[4] += nAmt2;
                            clsPmpaType.RPG.Amt3[50] += nAmt2;
                        }
                        #endregion
                    }
                    else if (nNu == 5 || nNu == 23)
                    {
                        #region 주사료
                        if (strBun == "20" || strBun == "21")
                        {
                            if (str100 == "Y")
                            {
                                clsPmpaType.RPG.Amt4[7] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (strGf == "Y")
                            {
                                clsPmpaType.RPG.Amt2[7] += nAmt1;
                                clsPmpaType.RPG.Amt2[50] += nAmt1;
                            }
                            clsPmpaType.RPG.Amt3[7] += nAmt2;
                            clsPmpaType.RPG.Amt3[50] += nAmt2;
                        }
                        else
                        {
                            if (str100 == "Y")
                            {
                                clsPmpaType.RPG.Amt4[6] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (strGf == "Y")
                            {
                                clsPmpaType.RPG.Amt2[6] += nAmt1;
                                clsPmpaType.RPG.Amt2[50] += nAmt1;
                            }
                            clsPmpaType.RPG.Amt3[6] += nAmt2;
                            clsPmpaType.RPG.Amt3[50] += nAmt2;
                        }
                        #endregion
                    }
                    else if (nNu == 6 || nNu == 24)
                    {
                        #region 마취료
                        if (str100 == "Y")
                        {
                            clsPmpaType.RPG.Amt4[8] += nAmt1;
                            clsPmpaType.RPG.Amt4[50] += nAmt1;
                        }
                        else if (strGf == "Y")
                        {
                            clsPmpaType.RPG.Amt2[8] += nAmt1;
                            clsPmpaType.RPG.Amt2[50] += nAmt1;
                        }
                        clsPmpaType.RPG.Amt3[8] += nAmt2;
                        clsPmpaType.RPG.Amt3[50] += nAmt2;
                        #endregion
                    }
                    else if (nNu == 9 || nNu == 10 || nNu == 12 || nNu == 27 || nNu == 28 || nNu == 30)     //처치 9
                    {
                        #region 처치료
                        if (str100 == "Y")
                        {
                            clsPmpaType.RPG.Amt4[9] += nAmt1;
                            clsPmpaType.RPG.Amt4[50] += nAmt1;
                        }
                        else if (strGf == "Y")
                        {
                            clsPmpaType.RPG.Amt2[9] += nAmt1;
                            clsPmpaType.RPG.Amt2[50] += nAmt1;
                        }
                        clsPmpaType.RPG.Amt3[9] += nAmt2;
                        clsPmpaType.RPG.Amt3[50] += nAmt2;
                        #endregion
                    }
                    else if (nNu == 13 || nNu == 14 || nNu == 31 || nNu == 32)     //검사 10
                    {
                        #region 검사료
                        if (str100 == "Y")
                        {
                            clsPmpaType.RPG.Amt4[10] += nAmt1;
                            clsPmpaType.RPG.Amt4[50] += nAmt1;
                        }
                        else if (strGf == "Y")
                        {
                            clsPmpaType.RPG.Amt2[10] += nAmt1;
                            clsPmpaType.RPG.Amt2[50] += nAmt1;
                        }
                        clsPmpaType.RPG.Amt3[10] += nAmt2;
                        clsPmpaType.RPG.Amt3[50] += nAmt2;
                        #endregion
                    }
                    else if (nNu == 15 || nNu == 33)     //영상진단 11
                    {
                        #region 영상진단
                        if (strBun == "65")
                        {
                            if (str100 == "Y")
                            {
                                clsPmpaType.RPG.Amt4[12] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (strGf == "Y")
                            {
                                clsPmpaType.RPG.Amt2[12] += nAmt1;
                                clsPmpaType.RPG.Amt2[50] += nAmt1;
                            }
                            clsPmpaType.RPG.Amt3[12] += nAmt2;
                            clsPmpaType.RPG.Amt3[50] += nAmt2;
                        }
                        else
                        {
                            if (str100 == "Y")
                            {
                                clsPmpaType.RPG.Amt4[11] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (strGf == "Y")
                            {
                                clsPmpaType.RPG.Amt2[11] += nAmt1;
                                clsPmpaType.RPG.Amt2[50] += nAmt1;
                            }
                            clsPmpaType.RPG.Amt3[11] += nAmt2;
                            clsPmpaType.RPG.Amt3[50] += nAmt2;
                        }
                        #endregion
                    }
                    else if (nNu == 7 || nNu == 25)  //물리치료 14
                    {
                        #region 물리치료
                        if (str100 == "Y")
                        {
                            clsPmpaType.RPG.Amt4[14] += nAmt1;
                            clsPmpaType.RPG.Amt4[50] += nAmt1;
                        }
                        else if (strGf == "Y")
                        {
                            clsPmpaType.RPG.Amt2[14] += nAmt1;
                            clsPmpaType.RPG.Amt2[50] += nAmt1;
                        }
                        clsPmpaType.RPG.Amt3[14] += nAmt2;
                        clsPmpaType.RPG.Amt3[50] += nAmt2;
                        #endregion
                    }
                    else if (nNu == 8 || nNu == 26) //정신요법 15
                    {
                        #region 정신요법
                        if (str100 == "Y")
                        {
                            clsPmpaType.RPG.Amt4[15] += nAmt1;
                            clsPmpaType.RPG.Amt4[50] += nAmt1;
                        }
                        else if (strGf == "Y")
                        {
                            clsPmpaType.RPG.Amt2[15] += nAmt1;
                            clsPmpaType.RPG.Amt2[50] += nAmt1;
                        }
                        clsPmpaType.RPG.Amt3[15] += nAmt2;
                        clsPmpaType.RPG.Amt3[50] += nAmt2;
                        #endregion
                    }
                    else if (nNu == 11 || nNu == 29) //수혈료 16
                    {
                        #region 수혈료
                        if (str100 == "Y")
                        {
                            clsPmpaType.RPG.Amt4[16] += nAmt1;
                            clsPmpaType.RPG.Amt4[50] += nAmt1;
                        }
                        else if (strGf == "Y")
                        {
                            clsPmpaType.RPG.Amt2[16] += nAmt1;
                            clsPmpaType.RPG.Amt2[50] += nAmt1;
                        }
                        clsPmpaType.RPG.Amt3[16] += nAmt2;
                        clsPmpaType.RPG.Amt3[50] += nAmt2;
                        #endregion
                    }
                    else if (nNu == 16 || nNu == 34)    //식대료
                    {
                        #region 식대료
                        if (strSelf == "0")
                        {
                            clsPmpaType.RPG.Amt1[3] += nAmt1;
                            clsPmpaType.RPG.Amt1[50] += nAmt1;
                        }
                        else if (strSelf == "1")
                        {
                            clsPmpaType.RPG.Amt2[3] += nAmt1;
                            clsPmpaType.RPG.Amt2[50] += nAmt1;
                        }
                        else
                        {
                            clsPmpaType.RPG.Amt4[3] += nAmt1;
                            clsPmpaType.RPG.Amt4[50] += nAmt1;
                        }
                        clsPmpaType.RPG.Amt3[3] += nAmt2;
                        clsPmpaType.RPG.Amt3[50] += nAmt2;
                        #endregion
                    }
                    else if (nNu == 35)     //병실차액
                    {
                        #region 병실차액
                        if (strSelf == "0")
                        {
                            clsPmpaType.RPG.Amt1[21] += nAmt1;
                            clsPmpaType.RPG.Amt1[50] += nAmt1;
                        }
                        else if (strSelf == "1")
                        {
                            clsPmpaType.RPG.Amt2[21] += nAmt1;
                            clsPmpaType.RPG.Amt2[50] += nAmt1;
                        }
                        else
                        {
                            clsPmpaType.RPG.Amt4[21] += nAmt1;
                            clsPmpaType.RPG.Amt4[50] += nAmt1;
                        }
                        clsPmpaType.RPG.Amt3[21] += nAmt2;
                        clsPmpaType.RPG.Amt3[50] += nAmt2;
                        #endregion
                    }
                    else if (nNu == 19 || nNu == 37) //CT 17
                    {
                        #region CT
                        if (str100 == "Y")
                        {
                            clsPmpaType.RPG.Amt4[17] += nAmt1;
                            clsPmpaType.RPG.Amt4[50] += nAmt1;
                        }
                        else if (strGf == "Y")
                        {
                            clsPmpaType.RPG.Amt2[17] += nAmt1;
                            clsPmpaType.RPG.Amt2[50] += nAmt1;
                        }
                        clsPmpaType.RPG.Amt3[17] += nAmt2;
                        clsPmpaType.RPG.Amt3[50] += nAmt2;
                        #endregion
                    }
                    else if (nNu == 18 || nNu == 38) //MRI 18
                    {
                        #region MRI
                        if (str100 == "Y")
                        {
                            clsPmpaType.RPG.Amt4[18] += nAmt1;
                            clsPmpaType.RPG.Amt4[50] += nAmt1;
                        }
                        else if (strGf == "Y")
                        {
                            clsPmpaType.RPG.Amt2[18] += nAmt1;
                            clsPmpaType.RPG.Amt2[50] += nAmt1;
                        }
                        clsPmpaType.RPG.Amt3[18] += nAmt2;
                        clsPmpaType.RPG.Amt3[50] += nAmt2;
                        #endregion
                    }
                    else if (nNu == 36) //초음파 19
                    {
                        #region 초음파
                        if (str100 == "Y")
                        {
                            clsPmpaType.RPG.Amt4[19] += nAmt1;
                            clsPmpaType.RPG.Amt4[50] += nAmt1;
                        }
                        else if (strGf == "Y")
                        {
                            clsPmpaType.RPG.Amt2[19] += nAmt1;
                            clsPmpaType.RPG.Amt2[50] += nAmt1;
                        }
                        clsPmpaType.RPG.Amt3[19] += nAmt2;
                        clsPmpaType.RPG.Amt3[50] += nAmt2;
                        #endregion
                    }
                    else if (nNu == 40) //보철교정 20
                    {
                        #region 보철교정
                        if (str100 == "Y")
                        {
                            clsPmpaType.RPG.Amt4[20] += nAmt1;
                            clsPmpaType.RPG.Amt4[50] += nAmt1;
                        }
                        else if (strGf == "Y")
                        {
                            clsPmpaType.RPG.Amt2[20] += nAmt1;
                            clsPmpaType.RPG.Amt2[50] += nAmt1;
                        }
                        clsPmpaType.RPG.Amt3[20] += nAmt2;
                        clsPmpaType.RPG.Amt3[50] += nAmt2;
                        #endregion
                    }
                    else if (nNu == 47) //증명료 22
                    {
                        #region 증명료
                        if (str100 == "Y")
                        {
                            clsPmpaType.RPG.Amt4[22] += nAmt1;
                            clsPmpaType.RPG.Amt4[50] += nAmt1;
                        }
                        else if (strGf == "Y")
                        {
                            clsPmpaType.RPG.Amt2[22] += nAmt1;
                            clsPmpaType.RPG.Amt2[50] += nAmt1;
                        }
                        clsPmpaType.RPG.Amt3[22] += nAmt2;
                        clsPmpaType.RPG.Amt3[50] += nAmt2;
                        #endregion
                    }
                    else if (nNu == 17 || nNu == 20 || nNu == 39 || nNu == 41 || nNu == 42 || nNu == 43 || nNu == 45 || nNu == 46 || nNu == 48) //기타 49
                    {
                        #region 기타
                        if (str100 == "Y")
                        {
                            clsPmpaType.RPG.Amt4[49] += nAmt1;
                            clsPmpaType.RPG.Amt4[50] += nAmt1;
                        }
                        else if (strGf == "Y")
                        {
                            clsPmpaType.RPG.Amt2[49] += nAmt1;
                            clsPmpaType.RPG.Amt2[50] += nAmt1;
                        }
                        clsPmpaType.RPG.Amt3[49] += nAmt2;
                        clsPmpaType.RPG.Amt3[50] += nAmt2;
                        #endregion
                    }
                }
                #endregion
            }

            Dt.Dispose();
            Dt = null;
            #endregion

            //인정비급여 100/100 다시 계산
            for (i = 0; i < 51; i++)
            {
                clsPmpaType.RPG.Amt4[i] = 0;  //본인총액합
            }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.NU,a.Bun,a.SUCODE,a.SUNEXT,         ";
            SQL += ComNum.VBLF + "        SUM(a.Amt1) Amt,SUM(a.Amt2) Amt2      ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a  ";
            SQL += ComNum.VBLF + "  WHERE  a.TRSNO = " + ArgTRSNO + "           ";
            SQL += ComNum.VBLF + "  GROUP BY a.NU,a.Bun,a.SUCODE,a.SUNEXT       ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            nRead = Dt.Rows.Count;

            if (nRead > 0)
            {
                for (i = 0; i < nRead; i++)
                {

                    strSugba = "";

                    //단일/복합/루틴 인진 판별
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT SugbA FROM " + ComNum.DB_PMPA + "BAS_SUT a, ";
                    SQL += ComNum.VBLF + "                   " + ComNum.DB_PMPA + "BAS_SUN b  ";
                    SQL += ComNum.VBLF + "  WHERE Sucode = '" + Dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";
                    SQL += ComNum.VBLF + "    AND a.SuNext = b.SuNext(+) ";
                    SQL += ComNum.VBLF + "    AND b.DRG100 = 'Y' ";
                    SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (Dt2.Rows.Count > 0)
                    {
                        strSugba = Dt2.Rows[0]["SugbA"].ToString().Trim();
                    }

                    Dt2.Dispose();
                    Dt2 = null;


                    if (DRG.Gn재왕절개본인부담율 > 0)
                    {
                        if (Dt.Rows[i]["SUCODE"].ToString().Trim() == "N-FT-PC" || Dt.Rows[i]["SUCODE"].ToString().Trim() == "N-FT10" || Dt.Rows[i]["SUCODE"].ToString().Trim() == "N-FE10" || Dt.Rows[i]["SUCODE"].ToString().Trim() == "N-FE-PC")
                            strSugba = "";
                    }

                    nNu = (int)VB.Val(Dt.Rows[i]["NU"].ToString().Trim());
                    strBun = Dt.Rows[i]["Bun"].ToString().Trim();

                    nAmt1 = (long)VB.Val(Dt.Rows[i]["Amt"].ToString().Trim());     //금액1

                    if (strSugba != "")   //100/100 세팅건만
                    {
                        if (strBun == "29" || strBun == "31" || strBun == "32" || strBun == "33" || strBun == "36" || strBun == "39")   //'치료재료대 13
                        {
                            clsPmpaType.RPG.Amt4[13] += nAmt1;
                            clsPmpaType.RPG.Amt4[50] += nAmt1;
                        }
                        else
                        {
                            if (nNu == 1)  //진찰료
                            {
                                clsPmpaType.RPG.Amt4[1] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (nNu == 2 || nNu == 3 || nNu == 21) //입원료
                            {
                                clsPmpaType.RPG.Amt4[2] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (nNu == 4 || nNu == 22) //투약 4
                            {
                                if (strBun == "11" || strBun == "12")
                                {
                                    clsPmpaType.RPG.Amt4[5] += nAmt1;
                                    clsPmpaType.RPG.Amt4[50] += nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[4] += nAmt1;
                                    clsPmpaType.RPG.Amt4[50] += nAmt1;
                                }
                            }
                            else if (nNu == 5 || nNu == 23) //주사 6
                            {
                                if (strBun == "20" || strBun == "21")
                                {
                                    clsPmpaType.RPG.Amt4[7] += nAmt1;
                                    clsPmpaType.RPG.Amt4[50] += nAmt1;
                                }
                                else
                                {
                                    clsPmpaType.RPG.Amt4[6] += nAmt1;
                                    clsPmpaType.RPG.Amt4[50] += nAmt1;
                                }
                            }
                            else if (nNu == 6 || nNu == 24) //마취료 8
                            {
                                clsPmpaType.RPG.Amt4[8] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (nNu == 9 || nNu == 10 || nNu == 12 || nNu == 27 || nNu == 28 || nNu == 30) //처치 9
                            {
                                clsPmpaType.RPG.Amt4[9] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (nNu == 13 || nNu == 14 || nNu == 31 || nNu == 32) //검사 10
                            {
                                clsPmpaType.RPG.Amt4[10] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (nNu == 15 || nNu == 33) //영상진단 11
                            {
                                clsPmpaType.RPG.Amt4[11] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;

                                if (strBun == "65")
                                {
                                    clsPmpaType.RPG.Amt4[12] += nAmt1;
                                    clsPmpaType.RPG.Amt4[50] += nAmt1;
                                }
                            }
                            else if (nNu == 7 || nNu == 25) //물리치료 14
                            {
                                clsPmpaType.RPG.Amt4[14] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (nNu == 8 || nNu == 26) //정신요법 15
                            {
                                clsPmpaType.RPG.Amt4[15] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (nNu == 11 || nNu == 29) //수혈료 16
                            {
                                clsPmpaType.RPG.Amt4[16] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (nNu == 19 || nNu == 37) //CT 17
                            {
                                clsPmpaType.RPG.Amt4[17] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (nNu == 18 || nNu == 38) //MRI 18
                            {
                                clsPmpaType.RPG.Amt4[18] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (nNu == 36) //초음파 19
                            {
                                clsPmpaType.RPG.Amt4[19] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (nNu == 40) //보철교정 20
                            {
                                clsPmpaType.RPG.Amt4[20] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (nNu == 47) //증명료 22
                            {
                                clsPmpaType.RPG.Amt4[22] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                            else if (nNu == 17 || nNu == 20 || nNu == 39 || nNu == 41 || nNu == 42 || nNu == 43 || nNu == 45 || nNu == 46 || nNu == 48) //기타 49
                            {
                                clsPmpaType.RPG.Amt4[49] += nAmt1;
                                clsPmpaType.RPG.Amt4[50] += nAmt1;
                            }
                        }
                    }
                }
            }

            Dt.Dispose();
            Dt = null;
        }

        /// <summary>
        /// Description : IPD_NEW_SLIP 읽어서 계산용 변수 세팅하기 RPG.AMT
        /// Author : 김민철
        /// Create Date : 2018.02.01
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPano"></param>
        /// <param name="IpdNo"></param>
        /// <param name="TrsNo"></param>
        /// <param name="ArgTempTrans"></param>
        /// <param name="ArgSDate"></param>
        /// <returns></returns>
        public bool Ipd_Tewon_PrtAmt_Gesan(PsmhDb pDbCon, string strPano, long IpdNo, long TrsNo, string ArgTempTrans, [Optional] string ArgSDate)
        {

            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, nRead = 0;
            int nNu = 0;
            long nTotGubyo = 0, nTotBiGubyo = 0;
            long nAmt = 0, nAMT09 = 0, nAMT85 = 0, nAMT09_H = 0, nAMT85_H = 0;
            long nBonGubyo = 0, nBonBiGubyo = 0, nBoninAmt = 0;
            long nCTMRAmt = 0, nCTMRBonin = 0, nBohoFood = 0, nFoodAmt = 0, nFoodGaAmt = 0, nFoodAmtBonin = 0, nFoodGaAmtBonin = 0, nFoodSumAmt = 0, nToothAmt = 0, nToothBonin = 0;
            long nTot100Amt = 0, n100Amt = 0, nKekliAmt = 0, nKekliAmt_Bon = 0, nRtnAmt = 0;
            long nTuberEduAmt = 0;      //2021-09-16 결핵관리료, 상담료
            long nHRoomAmt = 0,  nHRoomBonin = 0, nH1RoomAmt = 0;   //1,2인실 병실료 추가
            long nHUSetAmt = 0, nHUSetBonin = 0;   //호스피스 병실료 추가

            string strBDate = string.Empty;
            string strJuminNo = string.Empty;
            string strChild = string.Empty;
            string strMCode = string.Empty;
            string strOgPdBun = string.Empty;
            string strBun = string.Empty;
            string strDept = string.Empty;

            bool rtnVal = true;

            clsPmpaFunc cPF = new clsPmpaFunc();
            clsIpdAcct cIAcct = new clsIpdAcct();
            clsIuSentChk cISC = new clsIuSentChk();

            Read_Ipd_Mst_Trans(pDbCon, strPano, TrsNo, ArgTempTrans);

            #region 특정일자부터 항목별 금액을 재계산(의료급여 승인관련)
            if (ArgSDate != "")
            {
                for (i = 0; i < 51; i++)
                {
                    clsPmpaType.TIT.Amt[i] = 0;
                }

                SQL = "";
                SQL += ComNum.VBLF + " SELECT NU,SUM(Amt1) Amt,SUM(Amt2) Amt2 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO = " + TrsNo + " ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    SQL += ComNum.VBLF + "  AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD') ";
                }
                SQL += ComNum.VBLF + "  GROUP BY Nu ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        nNu = Convert.ToUInt16(Dt.Rows[i]["NU"].ToString());
                        clsPmpaType.TIT.Amt[nNu] += Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());
                        clsPmpaType.TIT.Amt[44] += Convert.ToInt64(Dt.Rows[i]["Amt2"].ToString());  //선택진료금액 재형성
                        clsPmpaType.TIT.Amt[50] += (Convert.ToInt64(Dt.Rows[i]["Amt"].ToString()) + Convert.ToInt64(Dt.Rows[i]["Amt2"].ToString()));
                    }
                }

                Dt.Dispose();
                Dt = null;
            }
            #endregion

            //인쇄용 개별항목값 변수 담기
            Ipd_Trans_PrtAmt_Read(pDbCon, TrsNo, ArgTempTrans);

            //총진료비중 급여총액, 비급여 총액을 계산함
            for (i = 1; i < 50; i++)
            {
                if (i >= 1 && i <= 20)
                {
                    nTotGubyo += clsPmpaType.TIT.Amt[i];        //급여분 합계
                }
                else
                {
                    nTotBiGubyo += clsPmpaType.TIT.Amt[i];    //비급여분 합계
                }
            }

            if (clsPmpaType.TIT.Amt[64] != 0)
            {
                nTotGubyo -= clsPmpaType.TIT.Amt[64];
            }

            #region 장기입원 대상자 입원료 본인부담 선별대상 구분 
            //입원료 본인부담율 선별구분되어짐으로 IPD_NEW_SLIP에 BonRate 컬럼 이용하는 방안 강구할것.
            //선별급여와 마찬가지로 SLIP, 수가별 본인부담율이 틀린것에 대한 구분자 표시 본인부담율 구하는 방법 강구할것.
            if (clsPmpaType.TIT.FCode != "F014" && VB.Left(clsPmpaType.TIT.Bi, 1) == "1" && clsPmpaType.TIT.VCode.Trim() == "" && clsPmpaType.TIT.OgPdBun.Trim() == "")
            {

                SQL = "";
                SQL += ComNum.VBLF + "  SELECT a.QTY,SUM(a.AMT1) Amt ";
                SQL += ComNum.VBLF + "    From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                SQL += ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_SUN b ";
                SQL += ComNum.VBLF + "   Where a.TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SQL += ComNum.VBLF + "     AND a.SUNEXT = b.SUNEXT(+)  ";
                SQL += ComNum.VBLF + "     AND b.DTLBUN = '1100' ";
                SQL += ComNum.VBLF + "     AND a.QTY <= 0.9 ";
                SQL += ComNum.VBLF + "     AND a.QTY >= 0.85  ";
                SQL += ComNum.VBLF + "     AND a.SUNEXT not in ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B','AV820A','AV820B','AV8201A','AV8201B') ";
                SQL += ComNum.VBLF + "   GROUP By a.QTY ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        nAmt = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());
                        if (Convert.ToDouble(Dt.Rows[i]["QTY"].ToString()) == 0.9)
                        {
                            nAMT09 = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString()); //본인부담 25%
                        }
                        else
                        {
                            nAMT85 = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString()); //본인부담 30%
                        }

                        nTotGubyo -= nAmt;
                    }
                }
                Dt.Dispose();
                Dt = null;
            }
            #endregion

            #region 장기입원 대상자 입원료 본인부담 선별대상 구분 상급병실
            //입원료 본인부담율 선별구분되어짐으로 IPD_NEW_SLIP에 BonRate 컬럼 이용하는 방안 강구할것.
            //선별급여와 마찬가지로 SLIP, 수가별 본인부담율이 틀린것에 대한 구분자 표시 본인부담율 구하는 방법 강구할것.
            if ( string.Compare(clsPmpaType.TIT.M_InDate, "2020-01-01") >= 0  && clsPmpaType.TIT.FCode != "F014" && VB.Left(clsPmpaType.TIT.Bi, 1) == "1" && clsPmpaType.TIT.VCode.Trim() == "" && clsPmpaType.TIT.OgPdBun.Trim() == "")
            {

                SQL = "";
                SQL += ComNum.VBLF + "  SELECT a.QTY,SUM(a.AMT1) Amt ";
                SQL += ComNum.VBLF + "    From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                SQL += ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_SUN b ";
                SQL += ComNum.VBLF + "   Where a.TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SQL += ComNum.VBLF + "     AND a.SUNEXT = b.SUNEXT(+)  ";
                SQL += ComNum.VBLF + "     AND b.DTLBUN = '1100' ";
                SQL += ComNum.VBLF + "     AND a.QTY <= 0.9 ";
                SQL += ComNum.VBLF + "     AND a.QTY >= 0.85  ";
                SQL += ComNum.VBLF + "     AND a.SUNEXT  in ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B') ";
                SQL += ComNum.VBLF + "   GROUP By a.QTY ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        nAmt = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());
                        if (Convert.ToDouble(Dt.Rows[i]["QTY"].ToString()) == 0.9)
                        {
                            nAMT09_H = (long)Math.Truncate(nAmt * 5 / 100.0);   //본인부담 5%가산   
                        }
                        else
                        {
                            nAMT85_H = (long)Math.Truncate(nAmt * 10 / 100.0);   //본인부담 10%가산   
                        }

                  
                    }
                }
                Dt.Dispose();
                Dt = null;
            }
            #endregion

            //항결핵약제비
            clsPmpaPb.GnAntiTubeDrug_Amt = 0;
            clsPmpaPb.GnHRoomAmt = 0;
            clsPmpaPb.GnHRoomBonin = 0;
            clsPmpaPb.GnH1RoomAmt = 0;
            clsPmpaPb.GnHUSetAmt = 0;
            clsPmpaPb.GnHUSetBonin = 0;

            #region //입원 본인부담율 세팅
            //기준일자 세팅
            cBON.SDATE = clsPmpaType.TIT.InDate;
            strJuminNo = clsPmpaType.TIT.Jumin1 + clsPmpaType.TIT.Jumin3;

            //나이구분 체크
            cBON.IO = "I";
            cBON.BI = clsPmpaType.TIT.Bi;
            cBON.CHILD = cPF.Acct_Age_Gubun(clsPmpaType.TIT.Age, strJuminNo, strBDate, cBON.IO);
            if (clsPmpaType.TIT.OgPdBun == "1")
            {
                cBON.MCODE = "E000";
                cBON.VCODE = "EV00";
            }
            else if (clsPmpaType.TIT.OgPdBun == "2")
            {
                cBON.MCODE = "F000";
                cBON.VCODE = "EV00";
            }
            else
            {
                cBON.VCODE = clsPmpaType.TIT.VCode;
                cBON.MCODE = cPF.Rtn_Ipd_OgPdBunDtl(clsPmpaType.TIT.OgPdBun);
            }
            //cBON.VCODE = clsPmpaType.TIT.VCode;
            //cBON.MCODE = cPF.Rtn_Ipd_OgPdBunDtl(clsPmpaType.TIT.OgPdBun);
            if (cPF.Rtn_Ipd_OgPdBunDtl(clsPmpaType.TIT.OgPdBun) == "")
            {
                cBON.OGPDBUN = clsPmpaType.TIT.OgPdBun;
            }
            else
            {
                cBON.OGPDBUN = clsPmpaType.TIT.OgPdBundtl;
            }
            cBON.DEPT = clsPmpaType.TIT.DeptCode;
            cBON.FCODE = clsPmpaType.TIT.FCode;

            //기본 부담율 계산
            if (cIAcct.Read_IBon_Rate(pDbCon, cBON) == false)
            {
                clsAlert cA = new ComPmpaLibB.clsAlert();
                cA.Alert_BonRate(cBON);
                return false;
            }

            if (clsPmpaType.TIT.Bohun == "3")   //장애인은 본인부담율 재조정 
            {
                if ((cBON.BI == "11" && cBON.MCODE == "F000") || cBON.BI == "22")
                {
                    clsPmpaType.IBR.Jin = 0;
                    clsPmpaType.IBR.Bohum = 0;
                    clsPmpaType.IBR.CTMRI = 0;
                }
            }

            #endregion

            #region 식대계산
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, SUM(AMT1) FoodAmt                      ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                        ";
            SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + "                                                    ";
            SQL += ComNum.VBLF + "    AND BUN IN ('74')                                                             ";
            SQL += ComNum.VBLF + "    AND NU  = '16'                                                                "; //식대 급여
            SQL += ComNum.VBLF + "    AND SUCODE NOT IN ('Y1110','T1110','Z4200','Z0100','Z0011','Z0010','Z0020')   ";
            SQL += ComNum.VBLF + "    AND SUBSTR(SUCODE, 1,1) NOT IN ('F')                                          "; //기존에 식대코드가 F로 시작한것 제외함."
            if (ArgTempTrans == "임시자격")
            {
                SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
            }
            else
            {
                if (ArgSDate != "")
                    SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
            }
            SQL += ComNum.VBLF + "  GROUP BY BDATE                                                                  ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
            nRead = Dt.Rows.Count;
            if (nRead > 0)
            {
                nFoodAmt = Convert.ToInt64(Dt.Rows[0]["FoodAmt"].ToString());

                for (i = 0; i < nRead; i++)
                {
                    strBDate = Dt.Rows[i]["BDATE"].ToString().Trim();
                    nFoodAmt = Convert.ToInt64(Dt.Rows[i]["FoodAmt"].ToString());
                    nFoodSumAmt += nFoodAmt;

                    if (string.Compare(strBDate, clsPmpaType.IBR.SDate) >= 0)
                    {
                        nFoodAmtBonin += (long)Math.Truncate(nFoodAmt * clsPmpaType.IBR.Food / 100.0);
                        clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(nFoodAmt * (clsPmpaType.IBR.Food / 100.0));
                    }
                }
            }

            Dt.Dispose();
            Dt = null;

            if (clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,SUM(Amt1) FoodAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND BUN IN ('74') ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                SQL += ComNum.VBLF + "    AND SUCODE = 'F02T' ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "  GROUP BY BDATE ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nAmt = Convert.ToInt64(Dt.Rows[i]["FoodAmt"].ToString());
                        if (clsPmpaType.TIT.VCode == "V191" || clsPmpaType.TIT.VCode == "V192" || clsPmpaType.TIT.VCode == "V268" || clsPmpaType.TIT.VCode == "V275")
                        {
                            nBohoFood = nBohoFood;            //2009-06-01 의료급여2종 중증 환자는 CT,MRI =>본인부담률이 0%
                        }
                        else
                        {
                            nBohoFood += nAmt;
                        }
                    }
                }
                Dt.Dispose();
                Dt = null;
            }

            #endregion

            #region 가산식대 계산
            SQL = "";
            SQL += ComNum.VBLF + " SELECT SUM(AMT1) FoodGaAmt ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
            SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + " ";
            SQL += ComNum.VBLF + "    AND BUN IN ('74') ";
            SQL += ComNum.VBLF + "    AND NU  = '16' "; //급여 식대
            SQL += ComNum.VBLF + "    AND SUCODE  IN ('Y1110','T1110','Z4200','Z0100','Z0011','Z0010','Z0020') ";
            if (ArgTempTrans == "임시자격")
            {
                SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
            }
            else
            {
                if (ArgSDate != "")
                    SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
            }
            SQL += ComNum.VBLF + "  GROUP BY  BUN ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
            nRead = Dt.Rows.Count;
            if (nRead > 0)
            {
                if (clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "21")
                    ComFunc.MsgBox("의료급여 환자에 가산식대가 발생하였습니다. 식대수가를 변경해주세요.", "확인");

                nFoodGaAmt = Convert.ToInt64(Dt.Rows[0]["FoodGaAmt"].ToString());

                if (clsPmpaType.TIT.Bi == "51" || clsPmpaType.TIT.Bi == "43" || clsPmpaType.TIT.Bi == "55" || clsPmpaType.TIT.Bi == "41" || clsPmpaType.TIT.Bi == "42")
                {
                    nFoodGaAmtBonin = (long)Math.Truncate((nFoodGaAmt * 100 / 100) * 0.1) * 10;                     //식대가산금액
                    clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(((nFoodGaAmt * 100 / 100) * 0.1) * 10);
                }
                else if (clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "33" || clsPmpaType.TIT.Bi == "31")
                {
                    nFoodGaAmtBonin = (long)Math.Truncate(((nFoodGaAmt * (clsPmpaType.IBR.Food / 100.0)) * 0.1) * 10);           //식대가산금액
                    clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(((nFoodGaAmt * (clsPmpaType.IBR.Food / 100.0)) * 0.1) * 10);
                }
                else
                {
                    if (clsPmpaType.TIT.OgPdBun == "P")
                    { nFoodGaAmtBonin = 0; }                                    //식대가산금액
                    else if (clsPmpaType.TIT.OgPdBun == "C")             //차상위계층환자는 가산식대 없음
                    { nFoodGaAmtBonin = 0; }                                     //식대가산금액
                    else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && (clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2"))
                    { nFoodGaAmtBonin = 0; }                                        //식대가산금액
                    else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && clsPmpaType.TIT.OgPdBun == "E")   //차상위계층2 만성질환자는 가산식대 (전액청구) 본인0%'2009 - 04 - 01
                    { nFoodGaAmtBonin = 0; }                                      //식대가산금액
                    else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && clsPmpaType.TIT.OgPdBun == "F")   //차상위계층2 장애인 만성질환자는 가산식대(전액청구) 본인0%'2009 - 04 - 01
                    { nFoodGaAmtBonin = 0; }                                        //식대가산금액
                    else
                    {
                        nFoodGaAmtBonin = (long)Math.Truncate(nFoodGaAmt * 0.5);    //식대가산금액
                        clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(nFoodGaAmt * 50 / 100.0);
                    }
                }
            }
            Dt.Dispose();
            Dt = null;
            #endregion

            #region 의뢰회신서 100% 급여
            //의뢰회신서 100%급여 2018-05-01
            if (VB.Left(clsPmpaType.TIT.Bi, 1) == "1")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate, SUM(Amt1) AMT100 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND SUCODE IN ( 'IA221','IA231' ) ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                if (ArgSDate != "")
                    SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                SQL += ComNum.VBLF + "  GROUP BY BDate ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["AMT100"].ToString()));
                        nRtnAmt += nAmt;
                    }
                }
                Dt.Dispose();
                Dt = null;
                clsPmpaType.RPG.Amt1[1] -= nRtnAmt;
                nTotGubyo -= nRtnAmt;
            }
            #endregion


            #region 결핵관리료, 상담료 100% 급여
            //결핵관리료, 상담료 100%급여 2021-09-16
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate, SUM(Amt1) AMT100 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
            SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + " ";
            SQL += ComNum.VBLF + "    AND SUCODE IN ( 'ID110','ID120','ID130' ) ";
            SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
            if (ArgSDate != "")
                SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
            SQL += ComNum.VBLF + "  GROUP BY BDate ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
            nRead = Dt.Rows.Count;
            if (nRead > 0)
            {
                for (i = 0; i < nRead; i++)
                {
                    nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["AMT100"].ToString()));
                    nTuberEduAmt += nAmt;
                }
            }
            Dt.Dispose();
            Dt = null;
            clsPmpaType.RPG.Amt1[1] -= nTuberEduAmt;
            nTotGubyo -= nTuberEduAmt;
            #endregion

            #region  퇴장방지 약제 가산단가 본인부담 제외

            if ((VB.Left(clsPmpaType.TIT.Bi, 1) == "1" || VB.Left(clsPmpaType.TIT.Bi, 1) == "2") && (clsPmpaType.TIT.DrgCode.Trim() =="" ))
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT round(SUM(a.AMT1) -  (SUM(a.AMT1) / 1.1) )   Amt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a ," + ComNum.DB_PMPA + "BAS_SUN b";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND a.SUNEXT = b.SUNEXT(+)";
                SQL += ComNum.VBLF + "    AND a.GbSelf ='0' ";
                SQL += ComNum.VBLF + "    AND b.sugbm = '1' ";
                SQL += ComNum.VBLF + "    AND a.BDate >= TO_DATE('2020-03-01','YYYY-MM-DD')         ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString()));
                        nTotGubyo -= nAmt;
                    }
                }
                Dt.Dispose();
                Dt = null;


            }
            #endregion

            #region CT,MRI 보험,보호는 외래 부담율을 적용함
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,Bun,SUM(Amt1) CTMRIAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND BUN IN ('72','73') ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                SQL += ComNum.VBLF + "    AND GbSUGBS ='0' ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "  GROUP BY BDATE, BUN ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strBDate = Dt.Rows[i]["BDATE"].ToString().Trim();
                        strBun = Dt.Rows[i]["Bun"].ToString().Trim();
                        nAmt = Convert.ToInt64(Dt.Rows[i]["CTMRIAmt"].ToString());
                        nCTMRAmt += nAmt;

                        if (string.Compare(strBDate, clsPmpaType.IBR.SDate) >= 0)
                        {
                            nCTMRBonin += (long)Math.Truncate(nAmt * clsPmpaType.IBR.CTMRI / 100.0);
                            if (strBun == "72")
                            { clsPmpaType.RPG.Amt5[17] += (long)Math.Truncate(nAmt * clsPmpaType.IBR.CTMRI / 100.0); }
                            else if (strBun == "73")
                            { clsPmpaType.RPG.Amt5[18] += (long)Math.Truncate(nAmt * clsPmpaType.IBR.CTMRI / 100.0); }
                        }
                    }
                }
                Dt.Dispose();
                Dt = null;
            }

            #endregion

            #region 2인실 병실료 
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0 )
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,Bun,SUM(Amt1) HRoomAmt  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND SUCode IN ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B','AV820A','AV820B','AV8201A','AV8201B')  ";   
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "  GROUP BY BDATE, BUN ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strBDate = Dt.Rows[i]["BDate"].ToString().Trim();
                        nAmt = Convert.ToInt64(Dt.Rows[i]["HRoomAmt"].ToString());
                        nHRoomAmt += nAmt;
                        nHRoomBonin += (long)Math.Truncate(nAmt * 40 / 100.0);
                       
                    }
                    clsPmpaPb.GnHRoomAmt = nHRoomAmt;
                    clsPmpaPb.GnHRoomBonin = nHRoomBonin + nAMT09_H + nAMT85_H ;

                }
                Dt.Dispose();
                Dt = null;
            }
            #endregion
           
            #region 1인실 비급여병실료  '2019-07-01' 
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,Bun,SUM(Amt1) H1RoomAmt  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND SUCode IN ('AB901')  ";
                SQL += ComNum.VBLF + "    AND BDATE >=TO_DATE('2019-07-01','YYYY-MM-DD') ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "  GROUP BY BDATE, BUN ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strBDate = Dt.Rows[i]["BDate"].ToString().Trim();
                        nAmt = Convert.ToInt64(Dt.Rows[i]["H1RoomAmt"].ToString());
                        nH1RoomAmt += nAmt;
                     

                    }
                    clsPmpaPb.GnH1RoomAmt = nH1RoomAmt;
                 

                }
                Dt.Dispose();
                Dt = null;
            }
            #endregion
            #region 호스피스 병실료  
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,Bun,SUM(Amt1) HRoomAmt  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO=" + TrsNo + " ";
                //SQL += ComNum.VBLF + "    AND SUCode IN ('WM211','WM271','WO211','WN211','WM202','WM262','WO202','WN202','WM221','WM281')  ";
                //7월부터 1등급으로 변경됨(2021-06-28)
                SQL += ComNum.VBLF + "    AND SUCode IN ('WM211','WM271','WO211','WN211','WM202','WM262','WO202','WN202','WM221','WM281','WM271')  ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "  GROUP BY BDATE, BUN ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strBDate = Dt.Rows[i]["BDate"].ToString().Trim();
                        nAmt = Convert.ToInt64(Dt.Rows[i]["HRoomAmt"].ToString());
                        nHUSetAmt += nAmt;
                        nHUSetBonin += (long)Math.Truncate(nAmt * clsPmpaType.IBR.Bohum / 100.0);  

                    }
                    clsPmpaPb.GnHUSetAmt = nHUSetAmt;
                    clsPmpaPb.GnHUSetBonin = nHUSetBonin;

                }
                Dt.Dispose();
                Dt = null;
            }
            #endregion

            #region 노인틀니 보험,보호 
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0 && clsPmpaType.TIT.DeptCode == "DT")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT b.DTLBUN,TO_CHAR(a.BDATE,'YYYY-MM-DD') BDate,a.Bun,SUM(a.Amt1) ToothAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND a.Pano = '" + strPano + "' ";
                SQL += ComNum.VBLF + "    AND a.SUNEXT = b.SUNEXT ";
                SQL += ComNum.VBLF + "    AND b.DTLBUN in ('4004','4003')  ";   //2017-07-12 기존 노인틀니에 임플란트('4003') 추가 (조건이 같음)
                SQL += ComNum.VBLF + "    AND a.GbSelf ='0' ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "  GROUP BY b.DTLBUN, a.BDATE, a.BUN ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strBDate = Dt.Rows[i]["BDate"].ToString().Trim();
                        nAmt = Convert.ToInt64(Dt.Rows[i]["ToothAmt"].ToString());
                        nToothAmt += nAmt;

                        if (string.Compare(strBDate, clsPmpaType.IBR.SDate) >= 0)
                        {
                            if (Dt.Rows[i]["DTLBUN"].ToString() == "4004")
                            {
                                nToothBonin += (long)Math.Truncate(nAmt * clsPmpaType.IBR.Dt1 / 100.0);
                               // clsPmpaType.RPG.Amt5[20] += (long)Math.Truncate(nAmt * clsPmpaType.IBR.Dt1 / 100.0);
                            }
                            else if (Dt.Rows[i]["DTLBUN"].ToString() == "4003")
                            {
                                nToothBonin += (long)Math.Truncate(nAmt * clsPmpaType.IBR.Dt2 / 100.0);
                                //clsPmpaType.RPG.Amt5[20] += (long)Math.Truncate(nAmt * clsPmpaType.IBR.Dt2 / 100.0);
                            }
                        }
                    }
                }
                Dt.Dispose();
                Dt = null;
                clsPmpaType.RPG.Amt1[49] -= nToothAmt;
            }
            #endregion

            #region TA환자 때문에 CT,MRI,SONO 변수에 저장함(비급여)
            if (clsPmpaType.TIT.Bi == "52")
            {
                nCTMRAmt = 0;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUM(Amt1) CTMRIAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN B ";
                SQL += ComNum.VBLF + "  WHERE A.TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND A.BUN IN ('71','72','73') ";
                SQL += ComNum.VBLF + "    AND A.GbSelf ='1' ";
                SQL += ComNum.VBLF + "    AND A.NU > '20' ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "    AND A.SUCODE = B.SUNEXT(+) ";
                SQL += ComNum.VBLF + "    AND SUGBR = '1' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["CTMRIAmt"].ToString()));
                        nCTMRAmt += nAmt;
                    }
                }
                Dt.Dispose();
                Dt = null;

                //보철료       
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUM(Amt1) CTMRIAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND BUN IN ('40') ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                SQL += ComNum.VBLF + "    AND NU > '20' ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["CTMRIAmt"].ToString()));
                        nCTMRAmt += nAmt;
                    }
                }
                Dt.Dispose();
                Dt = null;

            }
            #endregion

            string strSugbs = string.Empty;
            #region 100/100/80/50/90 본인일부 부담금중 조합부담금액 계산(급여본인부담금액에서 제외됨) 
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
            {
                //100/100 본인일부 부담금중 조합부담금액 계산(급여본인부담금액에서 제외됨) 2014-03-26
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUM(Amt1) Amt,GbSuGbs ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND GBSUGBS IN ('2','4','5','9') ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                SQL += ComNum.VBLF + "    AND BDate>=TO_DATE('2014-12-01','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  GROUP BY GbSuGbs ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strSugbs = Dt.Rows[i]["GbSuGbs"].ToString().Trim();
                        nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString()));
                        nTot100Amt += nAmt;

                        if (strSugbs == "4")
                            n100Amt += (long)Math.Truncate(nAmt * 0.8);       //본인부담 80%
                        else if (strSugbs == "5")
                            n100Amt += (long)Math.Truncate(nAmt * 0.5);       //본인부담 50%
                        else if (strSugbs == "2")
                            n100Amt += (long)Math.Truncate(nAmt * 0.2);       //본인부담 20%
                        else if (strSugbs == "9")
                            n100Amt += (long)Math.Truncate(nAmt * 0.9);       //본인부담 90%
                    }
                }
                Dt.Dispose();
                Dt = null;

                //100/80, 100/50 본인일부 부담금중 조합부담금액 계산(급여본인부담금액에서 제외됨) 2016-08-30
                SQL = "";
                SQL += ComNum.VBLF + " SELECT /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1) Amt,GbSuGbs ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO = " + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND GBSUGBS IN ('3','6','7','8') ";
                SQL += ComNum.VBLF + "    AND GbSelf = '0' ";
                SQL += ComNum.VBLF + "  GROUP BY GbSuGbs ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strSugbs = Dt.Rows[i]["GbSuGbs"].ToString().Trim();
                        nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString()));
                        nTot100Amt += nAmt;

                        if (strSugbs == "6")
                            n100Amt += (long)Math.Truncate(nAmt * 0.8);       //본인부담 80%
                        else if (strSugbs == "3")
                            n100Amt += (long)Math.Truncate(nAmt * 0.3);       //본인부담 30%
                        else if (strSugbs == "7")
                            n100Amt += (long)Math.Truncate(nAmt * 0.5);       //본인부담 50%
                        else if (strSugbs == "8")
                            n100Amt += (long)Math.Truncate(nAmt * 0.9);       //본인부담 90%
                    }
                }
                Dt.Dispose();
                Dt = null;

                //급여총액에서 100/80, 50, 90 총액을 제외함
                nTotGubyo = nTotGubyo - nTot100Amt;
            }
            #endregion

            #region 격리병실료 산정
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1) KekliAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND PANO ='" + clsPmpaType.TIT.Pano + "' ";
                SQL += ComNum.VBLF + "    AND SUCode IN ('AJ010','AJ020','AK200','AK201','AK202','AK210','AK211','V6001','V6002','AK200A','AH001','AH002') "; //  AK200A  2017-07-18 ADD
                SQL += ComNum.VBLF + "    AND BDate>=TO_DATE('2016-09-23','YYYY-MM-DD') ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["KekliAmt"].ToString()));

                        nKekliAmt += nAmt;

                        if (clsPmpaType.TIT.VCode != "")
                        {
                            if (clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13")
                            {
                                if (clsPmpaType.TIT.VCode == "V000" || clsPmpaType.TIT.VCode == "V010")
                                {
                                    nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.0);

                                }
                                else
                                {
                                    nKekliAmt_Bon += (long)Math.Truncate(nAmt * clsPmpaType.IBR.Bohum / 100.0);
                                }
                                   
                            }
                        }
                        else
                        {
                            if (clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13")
                            {
                                //F자격제외 실제로는 본인부담 없음
                                if (clsPmpaType.TIT.OgPdBun == "E" && clsPmpaType.TIT.Age < 6)
                                    nKekliAmt_Bon += 0;
                                else if(clsPmpaType.TIT.OgPdBun == "E" && clsPmpaType.TIT.Age <= 15)
                                    nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.03);
                                else if(clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2" || clsPmpaType.TIT.OgPdBun == "V" || clsPmpaType.TIT.OgPdBun == "H" || clsPmpaType.TIT.OgPdBun == "S" || clsPmpaType.TIT.OgPdBun == "Y")
                                    nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.05);
                                else if (clsPmpaType.TIT.OgPdBun == "F"|| clsPmpaType.TIT.OgPdBun == "C" || clsPmpaType.TIT.OgPdBun == "P")
                                    nKekliAmt_Bon = nKekliAmt_Bon;
                                else
                                    nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.1);
                            }
                            else if (clsPmpaType.TIT.Bi == "22"  && clsPmpaType.TIT.OgPdBun == "P")
                            {
                                nKekliAmt_Bon += 0;
                            }
                            else if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.OgPdBun == "Y" )
                            {
                                nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.03);
                            }
                            else if (clsPmpaType.TIT.Bi == "22")
                            {
                                //2021-06-17 의뢰서 적용
                                nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.1);
                                //nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.05);
                            }
                        }
                    }
                }
            }
            #endregion

            //항결핵약제비
            clsPmpaPb.GnAntiTubeDrug_Amt = cISC.Gesan_AntiTubeDrug_Amt(pDbCon, TrsNo);

            #region 본인부담액을 계산함
            //진료비 본인부담율
            if (clsPmpaType.TIT.Bi == "52" && clsPmpaType.TIT.Bi == "31" || clsPmpaType.TIT.Bi == "32")
            {
                nFoodGaAmt = 0;
            }

            nBonGubyo = (long)Math.Truncate((nTotGubyo - nCTMRAmt - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * clsPmpaType.IBR.Bohum / 100.0);

            //+항목별 본인부담율
            nBonGubyo += nCTMRBonin + nFoodAmtBonin + nFoodGaAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;

            //RPG.Amt5 본인부담금 구함
            Ipd_RPG_Amt_Set(pDbCon);

            nBonGubyo += (long)Math.Truncate(nAMT09 * 25 / 100.0);
            nBonGubyo += (long)Math.Truncate(nAMT85 * 30 / 100.0);

            //본인부담 상한제 계산
            if (string.Compare(clsPmpaType.TIT.Bi, "20") <= 0 && ArgSDate == "" && ArgTempTrans != "임시자격")
            {
                cIAcct.Gesan_Upper_Limit(pDbCon, ref nBonGubyo, ref nHRoomBonin);
            }

            #endregion
            clsPmpaType.RPG.Amt1[49] += nToothAmt;
            clsPmpaType.RPG.Amt5[49] += nToothBonin;
            clsPmpaType.RPG.Amt1[1] += nRtnAmt; //의뢰 회신사업 급여100% 2018-05-01 add
            clsPmpaType.RPG.Amt1[1] += nTuberEduAmt; //결핵관리료, 상담료 급여100% 2021-09-19

            #region 비급여 본인부담액을 계산
            switch (clsPmpaType.TIT.Bi)         //비급여분 본인 부담액 (자보: SONO, C/T, MRI,보철료는 제외)
            {
                case "52":
                    nBonBiGubyo = nTotBiGubyo - nCTMRAmt - clsPmpaType.TIT.Amt[36] - clsPmpaType.TIT.Amt[37] - clsPmpaType.TIT.Amt[38] - clsPmpaType.TIT.Amt[39] - clsPmpaType.TIT.Amt[44];
                    nBoninAmt = nBonGubyo + nBonBiGubyo;
                    break;
                default:
                    nBonBiGubyo = nTotBiGubyo;
                    nBoninAmt = nBonGubyo + nBonBiGubyo + n100Amt;
                    break;
            }

            for (i = 1; i < 25; i++)
            {
                clsPmpaType.RPG.Amt6[i] = clsPmpaType.RPG.Amt1[i] - clsPmpaType.RPG.Amt5[i];
            }

            clsPmpaType.RPG.Amt6[49] = clsPmpaType.RPG.Amt1[49] - clsPmpaType.RPG.Amt5[49]; //기타급여

            for (i = 1; i < 50; i++)
            {
                clsPmpaType.RPG.Amt5[50] += clsPmpaType.RPG.Amt5[i];
                clsPmpaType.RPG.Amt6[50] += clsPmpaType.RPG.Amt6[i];
            }

            clsPmpaType.TIT.Amt[51] = 0;
            clsPmpaType.TIT.Amt[52] = 0;

            if (clsPmpaType.TIT.Amt[64] != 0)
            {
                clsPmpaType.TIT.Amt[53] = clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64] - nBoninAmt;
            }
            else
            {
                clsPmpaType.TIT.Amt[53] = clsPmpaType.TIT.Amt[50] - nBoninAmt;
            }

           // clsPmpaType.TIT.Amt[61] = nBonGubyo;     //퇴원계산서
          //  clsPmpaType.TIT.Amt[62] = nBonBiGubyo;   //발부시 사용
            #endregion

            #region //할인금액 계산
            clsPmpaType.TIT.Amt[54] = 0;
            clsPmpaType.TIT.Amt[55] = nBoninAmt;     //차인납부액

            if (string.Compare(clsPmpaType.TIT.GbGameK, "00") > 0)
            {
                if (clsPmpaType.TIT.OutDate == "")
                {
                    if (clsPmpaType.TIT.GbGameK == "55" && clsPmpaType.TIT.GelCode == "")
                    {
                        clsPublic.GstrMsgList = "";
                        clsPublic.GstrMsgList += ComNum.VBLF + "계약처 감액인데 계약처 코드가 없습니다.";
                        clsPublic.GstrMsgList += ComNum.VBLF + "등록번호 : " + clsPmpaType.TIT.Pano;
                        clsPublic.GstrMsgList += ComNum.VBLF + "  진료과 : " + clsPmpaType.TIT.DeptCode;
                        ComFunc.MsgBox(clsPublic.GstrMsgList, "확인");
                    }

                    //2013-12-23 소방처전문치료 협약관련 감액기준
                    if (clsPmpaType.TIT.GelCode == "H911")
                    {
                        if (cIAcct.IPD_Gamek_Account_H119(pDbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPublic.GstrSysDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt) == false)
                            return false;

                        clsPmpaType.TIT.Amt[54] = clsPmpaType.GAM.Halin_Tot;
                        clsPmpaType.TIT.DtGamek = clsPmpaType.GAM.DTHalin_Tot;
                        clsPmpaType.TIT.Amt[55] = nBoninAmt - clsPmpaType.GAM.Halin_Tot;
                    }
                    else
                    {
                        if (cIAcct.IPD_Gamek_Account_Main(pDbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPublic.GstrSysDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt, nCTMRBonin) == false)
                            return false;
                    }
                }
                else
                {
                    //2013-12-23 소방처전문치료 협약관련 감액기준
                    if (clsPmpaType.TIT.GelCode == "H911")
                    {
                        if (cIAcct.IPD_Gamek_Account_H119(pDbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPublic.GstrSysDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt) == false)
                            return false;

                        clsPmpaType.TIT.Amt[54] = clsPmpaType.GAM.Halin_Tot;
                        clsPmpaType.TIT.DtGamek = clsPmpaType.GAM.DTHalin_Tot;
                        clsPmpaType.TIT.Amt[55] = nBoninAmt - clsPmpaType.GAM.Halin_Tot;
                    }
                    else
                    {
                        if (cIAcct.IPD_Gamek_Account_Main(pDbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPmpaType.TIT.OutDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt, nCTMRBonin) == false)
                            return false;
                    }
                }
                clsPmpaType.TIT.Amt[54] = clsPmpaType.GAM.Halin_Tot;
                clsPmpaType.TIT.DtGamek = clsPmpaType.GAM.DTHalin_Tot;
                clsPmpaType.TIT.Amt[55] = nBoninAmt - clsPmpaType.GAM.Halin_Tot;
            }
            #endregion

            cPF = null;

            return rtnVal;
        }



        public bool Ipd_Tewon_PrtAmt_Gesan_HU(PsmhDb pDbCon, string strPano, long IpdNo, long TrsNo, string ArgTempTrans, [Optional] string ArgSDate, [Optional] string ArgHUbyAct)
        {
            //2021-06-14 
            //호스피스 행위별 수가 금액 조회용 루틴입니다.
            //1) IPD_TRANS.GBHU = 'Y' 인것만 조회 가능합니다
            //   => (호스피스로 심사완료건만 조회됨)
            //2) PART = '!-' 만 조회
            //3) 금액에 "곱하기 -1"을 해줍니다.
            // 나머지는 메인 루틴과 동일하게 갑니다. 


            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, nRead = 0;
            int nNu = 0;
            long nTotGubyo = 0, nTotBiGubyo = 0;
            long nAmt = 0, nAMT09 = 0, nAMT85 = 0, nAMT09_H = 0, nAMT85_H = 0;
            long nBonGubyo = 0, nBonBiGubyo = 0, nBoninAmt = 0;
            long nCTMRAmt = 0, nCTMRBonin = 0, nBohoFood = 0, nFoodAmt = 0, nFoodGaAmt = 0, nFoodAmtBonin = 0, nFoodGaAmtBonin = 0, nFoodSumAmt = 0, nToothAmt = 0, nToothBonin = 0;
            long nTot100Amt = 0, n100Amt = 0, nKekliAmt = 0, nKekliAmt_Bon = 0, nRtnAmt = 0;
            long nTuberEduAmt = 0;      //2021-09-16 결핵관리료, 상담료
            long nHRoomAmt = 0, nHRoomBonin = 0, nH1RoomAmt = 0;   //1,2인실 병실료 추가
            long nHUSetAmt = 0, nHUSetBonin = 0;   //호스피스 병실료 추가

            string strBDate = string.Empty;
            string strJuminNo = string.Empty;
            string strChild = string.Empty;
            string strMCode = string.Empty;
            string strOgPdBun = string.Empty;
            string strBun = string.Empty;
            string strDept = string.Empty;

            bool rtnVal = true;

            clsPmpaFunc cPF = new clsPmpaFunc();
            clsIpdAcct cIAcct = new clsIpdAcct();
            clsIuSentChk cISC = new clsIuSentChk();

            Read_Ipd_Mst_Trans(pDbCon, strPano, TrsNo, ArgTempTrans);

            if (ArgHUbyAct == "1")
            {
                if (clsPmpaType.TIT.GBHU == "" || clsPmpaType.TIT.GBHU != "Y")
                {
                    ComFunc.MsgBox("호스피스 행위별 조회는 호스피스로 심사가 완료된 자격만 조회가 가능합니다.");
                    return false;
                }
            }

            #region 특정일자부터 항목별 금액을 재계산(의료급여 승인관련)
            if (ArgSDate != "")
            {
                for (i = 0; i < 51; i++)
                {
                    clsPmpaType.TIT.Amt[i] = 0;
                }

                SQL = "";
                SQL += ComNum.VBLF + " SELECT NU,SUM(Amt1 * -1) Amt,SUM(Amt2 * -1) Amt2 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO = " + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    SQL += ComNum.VBLF + "  AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD') ";
                }
                SQL += ComNum.VBLF + "  GROUP BY Nu ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        nNu = Convert.ToUInt16(Dt.Rows[i]["NU"].ToString());
                        clsPmpaType.TIT.Amt[nNu] += Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());
                        clsPmpaType.TIT.Amt[44] += Convert.ToInt64(Dt.Rows[i]["Amt2"].ToString());  //선택진료금액 재형성
                        clsPmpaType.TIT.Amt[50] += (Convert.ToInt64(Dt.Rows[i]["Amt"].ToString()) + Convert.ToInt64(Dt.Rows[i]["Amt2"].ToString()));
                    }
                }

                Dt.Dispose();
                Dt = null;
            }
            #endregion

            //인쇄용 개별항목값 변수 담기
            Ipd_Trans_PrtAmt_Read(pDbCon, TrsNo, ArgTempTrans, "1");

            //총진료비중 급여총액, 비급여 총액을 계산함
            for (i = 1; i < 50; i++)
            {
                if (i >= 1 && i <= 20)
                {
                    nTotGubyo += clsPmpaType.TIT.Amt[i];        //급여분 합계
                }
                else
                {
                    nTotBiGubyo += clsPmpaType.TIT.Amt[i];    //비급여분 합계
                }
            }

            if (clsPmpaType.TIT.Amt[64] != 0)
            {
                nTotGubyo -= clsPmpaType.TIT.Amt[64];
            }

            #region 장기입원 대상자 입원료 본인부담 선별대상 구분 
            //입원료 본인부담율 선별구분되어짐으로 IPD_NEW_SLIP에 BonRate 컬럼 이용하는 방안 강구할것.
            //선별급여와 마찬가지로 SLIP, 수가별 본인부담율이 틀린것에 대한 구분자 표시 본인부담율 구하는 방법 강구할것.
            if (clsPmpaType.TIT.FCode != "F014" && VB.Left(clsPmpaType.TIT.Bi, 1) == "1" && clsPmpaType.TIT.VCode.Trim() == "" && clsPmpaType.TIT.OgPdBun.Trim() == "")
            {

                SQL = "";
                SQL += ComNum.VBLF + "  SELECT a.QTY,SUM(a.AMT1 * -1) Amt ";
                SQL += ComNum.VBLF + "    From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                SQL += ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_SUN b ";
                SQL += ComNum.VBLF + "   Where a.TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
                SQL += ComNum.VBLF + "     AND a.SUNEXT = b.SUNEXT(+)  ";
                SQL += ComNum.VBLF + "     AND b.DTLBUN = '1100' ";
                SQL += ComNum.VBLF + "     AND a.QTY <= 0.9 ";
                SQL += ComNum.VBLF + "     AND a.QTY >= 0.85  ";
                SQL += ComNum.VBLF + "     AND a.SUNEXT not in ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B','AV820A','AV820B','AV8201A','AV8201B') ";
                SQL += ComNum.VBLF + "   GROUP By a.QTY ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        nAmt = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());
                        if (Convert.ToDouble(Dt.Rows[i]["QTY"].ToString()) == 0.9)
                        {
                            nAMT09 = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString()); //본인부담 25%
                        }
                        else
                        {
                            nAMT85 = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString()); //본인부담 30%
                        }

                        nTotGubyo -= nAmt;
                    }
                }
                Dt.Dispose();
                Dt = null;
            }
            #endregion

            #region 장기입원 대상자 입원료 본인부담 선별대상 구분 상급병실
            //입원료 본인부담율 선별구분되어짐으로 IPD_NEW_SLIP에 BonRate 컬럼 이용하는 방안 강구할것.
            //선별급여와 마찬가지로 SLIP, 수가별 본인부담율이 틀린것에 대한 구분자 표시 본인부담율 구하는 방법 강구할것.
            if (string.Compare(clsPmpaType.TIT.M_InDate, "2020-01-01") >= 0 && clsPmpaType.TIT.FCode != "F014" && VB.Left(clsPmpaType.TIT.Bi, 1) == "1" && clsPmpaType.TIT.VCode.Trim() == "" && clsPmpaType.TIT.OgPdBun.Trim() == "")
            {

                SQL = "";
                SQL += ComNum.VBLF + "  SELECT a.QTY,SUM(a.AMT1 * -1) Amt ";
                SQL += ComNum.VBLF + "    From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                SQL += ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_SUN b ";
                SQL += ComNum.VBLF + "   Where a.TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
                SQL += ComNum.VBLF + "     AND a.SUNEXT = b.SUNEXT(+)  ";
                SQL += ComNum.VBLF + "     AND b.DTLBUN = '1100' ";
                SQL += ComNum.VBLF + "     AND a.QTY <= 0.9 ";
                SQL += ComNum.VBLF + "     AND a.QTY >= 0.85  ";
                SQL += ComNum.VBLF + "     AND a.SUNEXT  in ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B') ";
                SQL += ComNum.VBLF + "   GROUP By a.QTY ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        nAmt = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());
                        if (Convert.ToDouble(Dt.Rows[i]["QTY"].ToString()) == 0.9)
                        {
                            nAMT09_H = (long)Math.Truncate(nAmt * 5 / 100.0);   //본인부담 5%가산   
                        }
                        else
                        {
                            nAMT85_H = (long)Math.Truncate(nAmt * 10 / 100.0);   //본인부담 10%가산   
                        }


                    }
                }
                Dt.Dispose();
                Dt = null;
            }
            #endregion

            //항결핵약제비
            clsPmpaPb.GnAntiTubeDrug_Amt = 0;
            clsPmpaPb.GnHRoomAmt = 0;
            clsPmpaPb.GnHRoomBonin = 0;
            clsPmpaPb.GnH1RoomAmt = 0;
            clsPmpaPb.GnHUSetAmt = 0;
            clsPmpaPb.GnHUSetBonin = 0;

            #region //입원 본인부담율 세팅
            //기준일자 세팅
            cBON.SDATE = clsPmpaType.TIT.InDate;
            strJuminNo = clsPmpaType.TIT.Jumin1 + clsPmpaType.TIT.Jumin3;

            //나이구분 체크
            cBON.IO = "I";
            cBON.BI = clsPmpaType.TIT.Bi;
            cBON.CHILD = cPF.Acct_Age_Gubun(clsPmpaType.TIT.Age, strJuminNo, strBDate, cBON.IO);
            if (clsPmpaType.TIT.OgPdBun == "1")
            {
                cBON.MCODE = "E000";
                cBON.VCODE = "EV00";
            }
            else if (clsPmpaType.TIT.OgPdBun == "2")
            {
                cBON.MCODE = "F000";
                cBON.VCODE = "EV00";
            }
            else
            {
                cBON.VCODE = clsPmpaType.TIT.VCode;
                cBON.MCODE = cPF.Rtn_Ipd_OgPdBunDtl(clsPmpaType.TIT.OgPdBun);
            }
            //cBON.VCODE = clsPmpaType.TIT.VCode;
            //cBON.MCODE = cPF.Rtn_Ipd_OgPdBunDtl(clsPmpaType.TIT.OgPdBun);
            if (cPF.Rtn_Ipd_OgPdBunDtl(clsPmpaType.TIT.OgPdBun) == "")
            {
                cBON.OGPDBUN = clsPmpaType.TIT.OgPdBun;
            }
            else
            {
                cBON.OGPDBUN = clsPmpaType.TIT.OgPdBundtl;
            }
            cBON.DEPT = clsPmpaType.TIT.DeptCode;
            cBON.FCODE = clsPmpaType.TIT.FCode;

            //기본 부담율 계산
            if (cIAcct.Read_IBon_Rate(pDbCon, cBON) == false)
            {
                clsAlert cA = new ComPmpaLibB.clsAlert();
                cA.Alert_BonRate(cBON);
                return false;
            }

            if (clsPmpaType.TIT.Bohun == "3")   //장애인은 본인부담율 재조정 
            {
                if ((cBON.BI == "11" && cBON.MCODE == "F000") || cBON.BI == "22")
                {
                    clsPmpaType.IBR.Jin = 0;
                    clsPmpaType.IBR.Bohum = 0;
                    clsPmpaType.IBR.CTMRI = 0;
                }
            }

            #endregion

            #region 식대계산
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, SUM(AMT1 * -1) FoodAmt                      ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                        ";
            SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + "                                                    ";
            SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
            SQL += ComNum.VBLF + "    AND BUN IN ('74')                                                             ";
            SQL += ComNum.VBLF + "    AND NU  = '16'                                                                "; //식대 급여
            SQL += ComNum.VBLF + "    AND SUCODE NOT IN ('Y1110','T1110','Z4200','Z0100','Z0011','Z0010','Z0020')   ";
            SQL += ComNum.VBLF + "    AND SUBSTR(SUCODE, 1,1) NOT IN ('F')                                          "; //기존에 식대코드가 F로 시작한것 제외함."
            if (ArgTempTrans == "임시자격")
            {
                SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
            }
            else
            {
                if (ArgSDate != "")
                    SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
            }
            SQL += ComNum.VBLF + "  GROUP BY BDATE                                                                  ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
            nRead = Dt.Rows.Count;
            if (nRead > 0)
            {
                nFoodAmt = Convert.ToInt64(Dt.Rows[0]["FoodAmt"].ToString());

                for (i = 0; i < nRead; i++)
                {
                    strBDate = Dt.Rows[i]["BDATE"].ToString().Trim();
                    nFoodAmt = Convert.ToInt64(Dt.Rows[i]["FoodAmt"].ToString());
                    nFoodSumAmt += nFoodAmt;

                    if (string.Compare(strBDate, clsPmpaType.IBR.SDate) >= 0)
                    {
                        nFoodAmtBonin += (long)Math.Truncate(nFoodAmt * clsPmpaType.IBR.Food / 100.0);
                        clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(nFoodAmt * (clsPmpaType.IBR.Food / 100.0));
                    }
                }
            }

            Dt.Dispose();
            Dt = null;

            if (clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,SUM(Amt1 * -1) FoodAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
                SQL += ComNum.VBLF + "    AND BUN IN ('74') ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                SQL += ComNum.VBLF + "    AND SUCODE = 'F02T' ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "  GROUP BY BDATE ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nAmt = Convert.ToInt64(Dt.Rows[i]["FoodAmt"].ToString());
                        if (clsPmpaType.TIT.VCode == "V191" || clsPmpaType.TIT.VCode == "V192" || clsPmpaType.TIT.VCode == "V268" || clsPmpaType.TIT.VCode == "V275")
                        {
                            nBohoFood = nBohoFood;            //2009-06-01 의료급여2종 중증 환자는 CT,MRI =>본인부담률이 0%
                        }
                        else
                        {
                            nBohoFood += nAmt;
                        }
                    }
                }
                Dt.Dispose();
                Dt = null;
            }

            #endregion

            #region 가산식대 계산
            SQL = "";
            SQL += ComNum.VBLF + " SELECT SUM(AMT1 * -1) FoodGaAmt ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
            SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + " ";
            SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
            SQL += ComNum.VBLF + "    AND BUN IN ('74') ";
            SQL += ComNum.VBLF + "    AND NU  = '16' "; //급여 식대
            SQL += ComNum.VBLF + "    AND SUCODE  IN ('Y1110','T1110','Z4200','Z0100','Z0011','Z0010','Z0020') ";
            if (ArgTempTrans == "임시자격")
            {
                SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
            }
            else
            {
                if (ArgSDate != "")
                    SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
            }
            SQL += ComNum.VBLF + "  GROUP BY  BUN ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
            nRead = Dt.Rows.Count;
            if (nRead > 0)
            {
                if (clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "21")
                    ComFunc.MsgBox("의료급여 환자에 가산식대가 발생하였습니다. 식대수가를 변경해주세요.", "확인");

                nFoodGaAmt = Convert.ToInt64(Dt.Rows[0]["FoodGaAmt"].ToString());

                if (clsPmpaType.TIT.Bi == "51" || clsPmpaType.TIT.Bi == "43" || clsPmpaType.TIT.Bi == "55" || clsPmpaType.TIT.Bi == "41" || clsPmpaType.TIT.Bi == "42")
                {
                    nFoodGaAmtBonin = (long)Math.Truncate((nFoodGaAmt * 100 / 100) * 0.1) * 10;                     //식대가산금액
                    clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(((nFoodGaAmt * 100 / 100) * 0.1) * 10);
                }
                else if (clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "33" || clsPmpaType.TIT.Bi == "31")
                {
                    nFoodGaAmtBonin = (long)Math.Truncate(((nFoodGaAmt * (clsPmpaType.IBR.Food / 100.0)) * 0.1) * 10);           //식대가산금액
                    clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(((nFoodGaAmt * (clsPmpaType.IBR.Food / 100.0)) * 0.1) * 10);
                }
                else
                {
                    if (clsPmpaType.TIT.OgPdBun == "P")
                    { nFoodGaAmtBonin = 0; }                                    //식대가산금액
                    else if (clsPmpaType.TIT.OgPdBun == "C")             //차상위계층환자는 가산식대 없음
                    { nFoodGaAmtBonin = 0; }                                     //식대가산금액
                    else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && (clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2"))
                    { nFoodGaAmtBonin = 0; }                                        //식대가산금액
                    else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && clsPmpaType.TIT.OgPdBun == "E")   //차상위계층2 만성질환자는 가산식대 (전액청구) 본인0%'2009 - 04 - 01
                    { nFoodGaAmtBonin = 0; }                                      //식대가산금액
                    else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && clsPmpaType.TIT.OgPdBun == "F")   //차상위계층2 장애인 만성질환자는 가산식대(전액청구) 본인0%'2009 - 04 - 01
                    { nFoodGaAmtBonin = 0; }                                        //식대가산금액
                    else
                    {
                        nFoodGaAmtBonin = (long)Math.Truncate(nFoodGaAmt * 0.5);    //식대가산금액
                        clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(nFoodGaAmt * 50 / 100.0);
                    }
                }
            }
            Dt.Dispose();
            Dt = null;
            #endregion

            #region 의뢰회신서 100% 급여
            //의뢰회신서 100%급여 2018-05-01
            if (VB.Left(clsPmpaType.TIT.Bi, 1) == "1")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate, SUM(Amt1 * -1) AMT100 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
                SQL += ComNum.VBLF + "    AND SUCODE IN ( 'IA221','IA231' ) ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                if (ArgSDate != "")
                    SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                SQL += ComNum.VBLF + "  GROUP BY BDate ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["AMT100"].ToString()));
                        nRtnAmt += nAmt;
                    }
                }
                Dt.Dispose();
                Dt = null;
                clsPmpaType.RPG.Amt1[1] -= nRtnAmt;
                nTotGubyo -= nRtnAmt;
            }
            #endregion

            #region 결핵관리,상담료 100% 급여
            //결핵관리 상담료 100%급여 2021-09-16
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate, SUM(Amt1 * -1) AMT100 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
            SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + " ";
            SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
            SQL += ComNum.VBLF + "    AND SUCODE IN ( 'ID110','ID120','ID130' ) ";
            SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
            if (ArgSDate != "")
                SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
            SQL += ComNum.VBLF + "  GROUP BY BDate ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
            nRead = Dt.Rows.Count;
            if (nRead > 0)
            {
                for (i = 0; i < nRead; i++)
                {
                    nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["AMT100"].ToString()));
                    nTuberEduAmt += nAmt;
                }
            }
            Dt.Dispose();
            Dt = null;
            clsPmpaType.RPG.Amt1[1] -= nTuberEduAmt;
            nTotGubyo -= nTuberEduAmt;
            #endregion


            #region  퇴장방지 약제 가산단가 본인부담 제외

            if ((VB.Left(clsPmpaType.TIT.Bi, 1) == "1" || VB.Left(clsPmpaType.TIT.Bi, 1) == "2") && (clsPmpaType.TIT.DrgCode.Trim() == ""))
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT round(SUM(a.AMT1 * -1) -  (SUM(a.AMT1 * -1) / 1.1) )   Amt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a ," + ComNum.DB_PMPA + "BAS_SUN b";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
                SQL += ComNum.VBLF + "    AND a.SUNEXT = b.SUNEXT(+)";
                SQL += ComNum.VBLF + "    AND a.GbSelf ='0' ";
                SQL += ComNum.VBLF + "    AND b.sugbm = '1' ";
                SQL += ComNum.VBLF + "    AND a.BDate >= TO_DATE('2020-03-01','YYYY-MM-DD')         ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString()));
                        nTotGubyo -= nAmt;
                    }
                }
                Dt.Dispose();
                Dt = null;


            }
            #endregion

            #region CT,MRI 보험,보호는 외래 부담율을 적용함
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,Bun,SUM(Amt1 * -1) CTMRIAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
                SQL += ComNum.VBLF + "    AND BUN IN ('72','73') ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                SQL += ComNum.VBLF + "    AND GbSUGBS ='0' ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "  GROUP BY BDATE, BUN ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strBDate = Dt.Rows[i]["BDATE"].ToString().Trim();
                        strBun = Dt.Rows[i]["Bun"].ToString().Trim();
                        nAmt = Convert.ToInt64(Dt.Rows[i]["CTMRIAmt"].ToString());
                        nCTMRAmt += nAmt;

                        if (string.Compare(strBDate, clsPmpaType.IBR.SDate) >= 0)
                        {
                            nCTMRBonin += (long)Math.Truncate(nAmt * clsPmpaType.IBR.CTMRI / 100.0);
                            if (strBun == "72")
                            { clsPmpaType.RPG.Amt5[17] += (long)Math.Truncate(nAmt * clsPmpaType.IBR.CTMRI / 100.0); }
                            else if (strBun == "73")
                            { clsPmpaType.RPG.Amt5[18] += (long)Math.Truncate(nAmt * clsPmpaType.IBR.CTMRI / 100.0); }
                        }
                    }
                }
                Dt.Dispose();
                Dt = null;
            }

            #endregion

            #region 2인실 병실료 
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,Bun,SUM(Amt1 * -1) HRoomAmt  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
                SQL += ComNum.VBLF + "    AND SUCode IN ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B','AV820A','AV820B','AV8201A','AV8201B')  ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "  GROUP BY BDATE, BUN ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strBDate = Dt.Rows[i]["BDate"].ToString().Trim();
                        nAmt = Convert.ToInt64(Dt.Rows[i]["HRoomAmt"].ToString());
                        nHRoomAmt += nAmt;
                        nHRoomBonin += (long)Math.Truncate(nAmt * 40 / 100.0);

                    }
                    clsPmpaPb.GnHRoomAmt = nHRoomAmt;
                    clsPmpaPb.GnHRoomBonin = nHRoomBonin + nAMT09_H + nAMT85_H;

                }
                Dt.Dispose();
                Dt = null;
            }
            #endregion

            #region 1인실 비급여병실료  '2019-07-01' 
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,Bun,SUM(Amt1 * -1) H1RoomAmt  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
                SQL += ComNum.VBLF + "    AND SUCode IN ('AB901')  ";
                SQL += ComNum.VBLF + "    AND BDATE >=TO_DATE('2019-07-01','YYYY-MM-DD') ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "  GROUP BY BDATE, BUN ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strBDate = Dt.Rows[i]["BDate"].ToString().Trim();
                        nAmt = Convert.ToInt64(Dt.Rows[i]["H1RoomAmt"].ToString());
                        nH1RoomAmt += nAmt;


                    }
                    clsPmpaPb.GnH1RoomAmt = nH1RoomAmt;


                }
                Dt.Dispose();
                Dt = null;
            }
            #endregion
            #region 호스피스 병실료  
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,Bun,SUM(Amt1 * -1) HRoomAmt  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
                //SQL += ComNum.VBLF + "    AND SUCode IN ('WM211','WM271','WO211','WN211','WM202','WM262','WO202','WN202','WM221','WM281')  ";
                //7월부터 1등급으로 변경됨(2021-06-28)
                SQL += ComNum.VBLF + "    AND SUCode IN ('WM211','WM271','WO211','WN211','WM202','WM262','WO202','WN202','WM221','WM281','WM271','WM211','WN211','WO211')  ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "  GROUP BY BDATE, BUN ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strBDate = Dt.Rows[i]["BDate"].ToString().Trim();
                        nAmt = Convert.ToInt64(Dt.Rows[i]["HRoomAmt"].ToString());
                        nHUSetAmt += nAmt;
                        nHUSetBonin += (long)Math.Truncate(nAmt * clsPmpaType.IBR.Bohum / 100.0);

                    }
                    clsPmpaPb.GnHUSetAmt = nHUSetAmt;
                    clsPmpaPb.GnHUSetBonin = nHUSetBonin;

                }
                Dt.Dispose();
                Dt = null;
            }
            #endregion

            #region 노인틀니 보험,보호 
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0 && clsPmpaType.TIT.DeptCode == "DT")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT b.DTLBUN,TO_CHAR(a.BDATE,'YYYY-MM-DD') BDate,a.Bun,SUM(a.Amt1 * -1) ToothAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
                SQL += ComNum.VBLF + "    AND a.Pano = '" + strPano + "' ";
                SQL += ComNum.VBLF + "    AND a.SUNEXT = b.SUNEXT ";
                SQL += ComNum.VBLF + "    AND b.DTLBUN in ('4004','4003')  ";   //2017-07-12 기존 노인틀니에 임플란트('4003') 추가 (조건이 같음)
                SQL += ComNum.VBLF + "    AND a.GbSelf ='0' ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "  GROUP BY b.DTLBUN, a.BDATE, a.BUN ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strBDate = Dt.Rows[i]["BDate"].ToString().Trim();
                        nAmt = Convert.ToInt64(Dt.Rows[i]["ToothAmt"].ToString());
                        nToothAmt += nAmt;

                        if (string.Compare(strBDate, clsPmpaType.IBR.SDate) >= 0)
                        {
                            if (Dt.Rows[i]["DTLBUN"].ToString() == "4004")
                            {
                                nToothBonin += (long)Math.Truncate(nAmt * clsPmpaType.IBR.Dt1 / 100.0);
                                // clsPmpaType.RPG.Amt5[20] += (long)Math.Truncate(nAmt * clsPmpaType.IBR.Dt1 / 100.0);
                            }
                            else if (Dt.Rows[i]["DTLBUN"].ToString() == "4003")
                            {
                                nToothBonin += (long)Math.Truncate(nAmt * clsPmpaType.IBR.Dt2 / 100.0);
                                //clsPmpaType.RPG.Amt5[20] += (long)Math.Truncate(nAmt * clsPmpaType.IBR.Dt2 / 100.0);
                            }
                        }
                    }
                }
                Dt.Dispose();
                Dt = null;
                clsPmpaType.RPG.Amt1[49] -= nToothAmt;
            }
            #endregion

            #region TA환자 때문에 CT,MRI,SONO 변수에 저장함(비급여)
            if (clsPmpaType.TIT.Bi == "52")
            {
                nCTMRAmt = 0;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUM(Amt1 * -1) CTMRIAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN B ";
                SQL += ComNum.VBLF + "  WHERE A.TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
                SQL += ComNum.VBLF + "    AND A.BUN IN ('71','72','73') ";
                SQL += ComNum.VBLF + "    AND A.GbSelf ='1' ";
                SQL += ComNum.VBLF + "    AND A.NU > '20' ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "    AND A.SUCODE = B.SUNEXT(+) ";
                SQL += ComNum.VBLF + "    AND SUGBR = '1' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["CTMRIAmt"].ToString()));
                        nCTMRAmt += nAmt;
                    }
                }
                Dt.Dispose();
                Dt = null;

                //보철료       
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUM(Amt1 * -1) CTMRIAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
                SQL += ComNum.VBLF + "    AND BUN IN ('40') ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                SQL += ComNum.VBLF + "    AND NU > '20' ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["CTMRIAmt"].ToString()));
                        nCTMRAmt += nAmt;
                    }
                }
                Dt.Dispose();
                Dt = null;

            }
            #endregion

            string strSugbs = string.Empty;
            #region 100/100/80/50/90 본인일부 부담금중 조합부담금액 계산(급여본인부담금액에서 제외됨) 
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
            {
                //100/100 본인일부 부담금중 조합부담금액 계산(급여본인부담금액에서 제외됨) 2014-03-26
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUM(Amt1 * -1) Amt,GbSuGbs ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
                SQL += ComNum.VBLF + "    AND GBSUGBS IN ('2','4','5','9') ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                SQL += ComNum.VBLF + "    AND BDate>=TO_DATE('2014-12-01','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  GROUP BY GbSuGbs ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strSugbs = Dt.Rows[i]["GbSuGbs"].ToString().Trim();
                        nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString()));
                        nTot100Amt += nAmt;

                        if (strSugbs == "4")
                            n100Amt += (long)Math.Truncate(nAmt * 0.8);       //본인부담 80%
                        else if (strSugbs == "5")
                            n100Amt += (long)Math.Truncate(nAmt * 0.5);       //본인부담 50%
                        else if (strSugbs == "2")
                            n100Amt += (long)Math.Truncate(nAmt * 0.2);       //본인부담 20%
                        else if (strSugbs == "9")
                            n100Amt += (long)Math.Truncate(nAmt * 0.9);       //본인부담 90%
                    }
                }
                Dt.Dispose();
                Dt = null;

                //100/80, 100/50 본인일부 부담금중 조합부담금액 계산(급여본인부담금액에서 제외됨) 2016-08-30
                SQL = "";
                SQL += ComNum.VBLF + " SELECT /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1 * -1) Amt,GbSuGbs ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO = " + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
                SQL += ComNum.VBLF + "    AND GBSUGBS IN ('3','6','7','8') ";
                SQL += ComNum.VBLF + "    AND GbSelf = '0' ";
                SQL += ComNum.VBLF + "  GROUP BY GbSuGbs ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strSugbs = Dt.Rows[i]["GbSuGbs"].ToString().Trim();
                        nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString()));
                        nTot100Amt += nAmt;

                        if (strSugbs == "6")
                            n100Amt += (long)Math.Truncate(nAmt * 0.8);       //본인부담 80%
                        else if (strSugbs == "3")
                            n100Amt += (long)Math.Truncate(nAmt * 0.3);       //본인부담 30%
                        else if (strSugbs == "7")
                            n100Amt += (long)Math.Truncate(nAmt * 0.5);       //본인부담 50%
                        else if (strSugbs == "8")
                            n100Amt += (long)Math.Truncate(nAmt * 0.9);       //본인부담 90%
                    }
                }
                Dt.Dispose();
                Dt = null;

                //급여총액에서 100/80, 50, 90 총액을 제외함
                nTotGubyo = nTotGubyo - nTot100Amt;
            }
            #endregion

            #region 격리병실료 산정
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1 * -1) KekliAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + TrsNo + " ";
                SQL += ComNum.VBLF + "    AND TRIM(PART) = '!-' ";
                SQL += ComNum.VBLF + "    AND PANO ='" + clsPmpaType.TIT.Pano + "' ";
                SQL += ComNum.VBLF + "    AND SUCode IN ('AJ010','AJ020','AK200','AK201','AK202','AK210','AK211','V6001','V6002','AK200A','AH001','AH002') "; //  AK200A  2017-07-18 ADD
                SQL += ComNum.VBLF + "    AND BDate>=TO_DATE('2016-09-23','YYYY-MM-DD') ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["KekliAmt"].ToString()));

                        nKekliAmt += nAmt;

                        if (clsPmpaType.TIT.VCode != "")
                        {
                            if (clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13")
                            {
                                if (clsPmpaType.TIT.VCode == "V000" || clsPmpaType.TIT.VCode == "V010")
                                {
                                    nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.0);

                                }
                                else
                                {
                                    nKekliAmt_Bon += (long)Math.Truncate(nAmt * clsPmpaType.IBR.Bohum / 100.0);
                                }

                            }
                        }
                        else
                        {
                            if (clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13")
                            {
                                //F자격제외 실제로는 본인부담 없음
                                if (clsPmpaType.TIT.OgPdBun == "E" && clsPmpaType.TIT.Age < 6)
                                    nKekliAmt_Bon += 0;
                                else if (clsPmpaType.TIT.OgPdBun == "E" && clsPmpaType.TIT.Age <= 15)
                                    nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.03);
                                else if (clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2" || clsPmpaType.TIT.OgPdBun == "V" || clsPmpaType.TIT.OgPdBun == "H" || clsPmpaType.TIT.OgPdBun == "S" || clsPmpaType.TIT.OgPdBun == "Y")
                                    nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.05);
                                else if (clsPmpaType.TIT.OgPdBun == "F" || clsPmpaType.TIT.OgPdBun == "C" || clsPmpaType.TIT.OgPdBun == "P")
                                    nKekliAmt_Bon = nKekliAmt_Bon;
                                else
                                    nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.1);
                            }
                            else if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.OgPdBun == "P")
                            {
                                nKekliAmt_Bon += 0;
                            }
                            else if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.OgPdBun == "Y")
                            {
                                nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.03);
                            }
                            else if (clsPmpaType.TIT.Bi == "22")
                            {
                                //2021-06-17 의뢰서
                                nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.1);
                                //nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.05);
                            }
                        }
                    }
                }
            }
            #endregion

            //항결핵약제비
            clsPmpaPb.GnAntiTubeDrug_Amt = cISC.Gesan_AntiTubeDrug_Amt(pDbCon, TrsNo);

            #region 본인부담액을 계산함
            //진료비 본인부담율
            if (clsPmpaType.TIT.Bi == "52" && clsPmpaType.TIT.Bi == "31" || clsPmpaType.TIT.Bi == "32")
            {
                nFoodGaAmt = 0;
            }

            nBonGubyo = (long)Math.Truncate((nTotGubyo - nCTMRAmt - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * clsPmpaType.IBR.Bohum / 100.0);

            //+항목별 본인부담율
            nBonGubyo += nCTMRBonin + nFoodAmtBonin + nFoodGaAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;

            //RPG.Amt5 본인부담금 구함
            Ipd_RPG_Amt_Set(pDbCon);

            nBonGubyo += (long)Math.Truncate(nAMT09 * 25 / 100.0);
            nBonGubyo += (long)Math.Truncate(nAMT85 * 30 / 100.0);

            //본인부담 상한제 계산
            if (string.Compare(clsPmpaType.TIT.Bi, "20") <= 0 && ArgSDate == "" && ArgTempTrans != "임시자격")
            {
                cIAcct.Gesan_Upper_Limit(pDbCon, ref nBonGubyo, ref nHRoomBonin);
            }

            #endregion
            clsPmpaType.RPG.Amt1[49] += nToothAmt;
            clsPmpaType.RPG.Amt5[49] += nToothBonin;
            clsPmpaType.RPG.Amt1[1] += nRtnAmt; //의뢰 회신사업 급여100% 2018-05-01 add
            clsPmpaType.RPG.Amt1[1] += nTuberEduAmt; //결핵관리료,상담료 급여100% 2021-09-27 add

            #region 비급여 본인부담액을 계산
            switch (clsPmpaType.TIT.Bi)         //비급여분 본인 부담액 (자보: SONO, C/T, MRI,보철료는 제외)
            {
                case "52":
                    nBonBiGubyo = nTotBiGubyo - nCTMRAmt - clsPmpaType.TIT.Amt[36] - clsPmpaType.TIT.Amt[37] - clsPmpaType.TIT.Amt[38] - clsPmpaType.TIT.Amt[39] - clsPmpaType.TIT.Amt[44];
                    nBoninAmt = nBonGubyo + nBonBiGubyo;
                    break;
                default:
                    nBonBiGubyo = nTotBiGubyo;
                    nBoninAmt = nBonGubyo + nBonBiGubyo + n100Amt;
                    break;
            }

            for (i = 1; i < 25; i++)
            {
                clsPmpaType.RPG.Amt6[i] = clsPmpaType.RPG.Amt1[i] - clsPmpaType.RPG.Amt5[i];
            }

            clsPmpaType.RPG.Amt6[49] = clsPmpaType.RPG.Amt1[49] - clsPmpaType.RPG.Amt5[49]; //기타급여

            for (i = 1; i < 50; i++)
            {
                clsPmpaType.RPG.Amt5[50] += clsPmpaType.RPG.Amt5[i];
                clsPmpaType.RPG.Amt6[50] += clsPmpaType.RPG.Amt6[i];
            }

            clsPmpaType.TIT.Amt[51] = 0;
            clsPmpaType.TIT.Amt[52] = 0;

            if (clsPmpaType.TIT.Amt[64] != 0)
            {
                clsPmpaType.TIT.Amt[53] = clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64] - nBoninAmt;
            }
            else
            {
                clsPmpaType.TIT.Amt[53] = clsPmpaType.TIT.Amt[50] - nBoninAmt;
            }

            // clsPmpaType.TIT.Amt[61] = nBonGubyo;     //퇴원계산서
            //  clsPmpaType.TIT.Amt[62] = nBonBiGubyo;   //발부시 사용
            #endregion

            #region //할인금액 계산
            clsPmpaType.TIT.Amt[54] = 0;
            clsPmpaType.TIT.Amt[55] = nBoninAmt;     //차인납부액

            if (string.Compare(clsPmpaType.TIT.GbGameK, "00") > 0)
            {
                if (clsPmpaType.TIT.OutDate == "")
                {
                    if (clsPmpaType.TIT.GbGameK == "55" && clsPmpaType.TIT.GelCode == "")
                    {
                        clsPublic.GstrMsgList = "";
                        clsPublic.GstrMsgList += ComNum.VBLF + "계약처 감액인데 계약처 코드가 없습니다.";
                        clsPublic.GstrMsgList += ComNum.VBLF + "등록번호 : " + clsPmpaType.TIT.Pano;
                        clsPublic.GstrMsgList += ComNum.VBLF + "  진료과 : " + clsPmpaType.TIT.DeptCode;
                        ComFunc.MsgBox(clsPublic.GstrMsgList, "확인");
                    }

                    //2013-12-23 소방처전문치료 협약관련 감액기준
                    if (clsPmpaType.TIT.GelCode == "H911")
                    {
                        if (cIAcct.IPD_Gamek_Account_H119(pDbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPublic.GstrSysDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt) == false)
                            return false;

                        clsPmpaType.TIT.Amt[54] = clsPmpaType.GAM.Halin_Tot;
                        clsPmpaType.TIT.DtGamek = clsPmpaType.GAM.DTHalin_Tot;
                        clsPmpaType.TIT.Amt[55] = nBoninAmt - clsPmpaType.GAM.Halin_Tot;
                    }
                    else
                    {
                        if (cIAcct.IPD_Gamek_Account_Main(pDbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPublic.GstrSysDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt, nCTMRBonin) == false)
                            return false;
                    }
                }
                else
                {
                    //2013-12-23 소방처전문치료 협약관련 감액기준
                    if (clsPmpaType.TIT.GelCode == "H911")
                    {
                        if (cIAcct.IPD_Gamek_Account_H119(pDbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPublic.GstrSysDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt) == false)
                            return false;

                        clsPmpaType.TIT.Amt[54] = clsPmpaType.GAM.Halin_Tot;
                        clsPmpaType.TIT.DtGamek = clsPmpaType.GAM.DTHalin_Tot;
                        clsPmpaType.TIT.Amt[55] = nBoninAmt - clsPmpaType.GAM.Halin_Tot;
                    }
                    else
                    {
                        if (cIAcct.IPD_Gamek_Account_Main(pDbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPmpaType.TIT.OutDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt, nCTMRBonin) == false)
                            return false;
                    }
                }
                clsPmpaType.TIT.Amt[54] = clsPmpaType.GAM.Halin_Tot;
                clsPmpaType.TIT.DtGamek = clsPmpaType.GAM.DTHalin_Tot;
                clsPmpaType.TIT.Amt[55] = nBoninAmt - clsPmpaType.GAM.Halin_Tot;
            }
            #endregion

            cPF = null;

            return rtnVal;
        }


        /// <summary>
        /// 연말정산용
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPano"></param>
        /// <param name="IpdNo"></param>
        /// <param name="TrsNo"></param>
        /// <param name="ArgTempTrans"></param>
        /// <param name="ArgSDate"></param>
        /// <returns></returns>
        public bool Ipd_Tewon_PrtAmt_Gesan_Junsan(PsmhDb pDbCon, string strPano, long IpdNo, long ArgTRSNO, string ArgTempTrans, [Optional] string ArgSDate)
        {

            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, nRead = 0;
            int nNu = 0;
            long nTotGubyo = 0, nTotBiGubyo = 0;
            long nAmt = 0, nAMT09 = 0, nAMT85 = 0,  nAMT09_H = 0, nAMT85_H = 0;
            long nBonGubyo = 0, nBonBiGubyo = 0, nBoninAmt = 0;
            long nCTMRAmt = 0, nCTMRBonin = 0, nBohoFood = 0, nFoodAmt = 0, nFoodGaAmt = 0, nFoodAmtBonin = 0, nFoodGaAmtBonin = 0, nFoodSumAmt = 0, nToothAmt = 0, nToothBonin = 0;
            long nTot100Amt = 0, n100Amt = 0, nKekliAmt = 0, nKekliAmt_Bon = 0, nRtnAmt = 0;
            long nHRoomAmt = 0, nHRoomBonin = 0;   //2인실 병실료 추가
            long nHUSetAmt = 0, nHUSetBonin = 0;   //호스피스 병실료 추가

            string strBDate = string.Empty;
            string strBBDate = string.Empty;
            string strJuminNo = string.Empty;
            string strChild = string.Empty;
            string strMCode = string.Empty;
            string strOgPdBun = string.Empty;
            string strBun = string.Empty;
            string strDept = string.Empty;

            bool rtnVal = true;

            int nOpdBonRate = 0;
            int nBonRate = 0;

            long GnTempAmt_Bon = 0;
            long GnTempHRoomBonin = 0;

            clsPmpaFunc cPF = new clsPmpaFunc();
            clsIpdAcct cIAcct = new clsIpdAcct();
            clsIuSentChk cISC = new clsIuSentChk();

            if (ArgSDate == null)
            {
                ArgSDate = "";
            }

            if (ArgTempTrans != "임시자격")
            {
                //Read_Ipd_Mst_Trans(pDbCon, strPano, ArgTRSNO, ArgTempTrans);
                READ_IPD_TRANS_Junsan(pDbCon, ArgTRSNO, 0);
            }

            #region 특정일자부터 항목별 금액을 재계산(의료급여 승인관련)
            if (ArgSDate != "")
            {
                for (i = 0; i < 51; i++)
                {
                    clsPmpaType.TIT.Amt[i] = 0;
                }

                SQL = "";
                SQL += ComNum.VBLF + " SELECT NU,SUM(Amt1) Amt,SUM(Amt2) Amt2 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO = " + ArgTRSNO + " ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    SQL += ComNum.VBLF + "  AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD') ";
                }
                SQL += ComNum.VBLF + "  GROUP BY Nu ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        nNu = Convert.ToUInt16(Dt.Rows[i]["NU"].ToString());
                        clsPmpaType.TIT.Amt[nNu] += Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());
                        clsPmpaType.TIT.Amt[44] += Convert.ToInt64(Dt.Rows[i]["Amt2"].ToString());  //선택진료금액 재형성
                        clsPmpaType.TIT.Amt[50] += (Convert.ToInt64(Dt.Rows[i]["Amt"].ToString()) + Convert.ToInt64(Dt.Rows[i]["Amt2"].ToString()));
                    }
                }

                Dt.Dispose();
                Dt = null;
            }
            #endregion

            //인쇄용 개별항목값 변수 담기
            if (ArgTempTrans != "임시자격")
            {
                if (clsPmpaPb.Gstr누적계산New == "OK")
                {
                    Ipd_Trans_PrtAmt_Read_New_Junsan(pDbCon, ArgTRSNO, ArgTempTrans);
                }
                else
                {
                    Ipd_Trans_PrtAmt_Read_Junsan(pDbCon, ArgTRSNO, ArgTempTrans);
                }
            }
            else
            {
                Ipd_Trans_PrtAmt_Read_Junsan(pDbCon, ArgTRSNO, ArgTempTrans);
            }
            

            //총진료비중 급여총액, 비급여 총액을 계산함
            for (i = 1; i < 50; i++)
            {
                if (i >= 1 && i <= 20)
                {
                    nTotGubyo += clsPmpaType.TIT.Amt[i];        //급여분 합계
                }
                else
                {
                    nTotBiGubyo += clsPmpaType.TIT.Amt[i];    //비급여분 합계
                }
            }

            if (clsPmpaType.TIT.Amt[64] != 0)
            {
                clsPmpaType.RPG.Amt1[4] = clsPmpaType.RPG.Amt1[4] - clsPmpaType.TIT.Amt[64];
            }

            if (clsPmpaType.TIT.Amt[64] != 0)
            {
                nTotGubyo -= clsPmpaType.TIT.Amt[64];
            }

            nAmt = 0; nAMT09 = 0; nAMT85 = 0;
            #region 장기입원 대상자 입원료 본인부담 선별대상 구분 
            //입원료 본인부담율 선별구분되어짐으로 IPD_NEW_SLIP에 BonRate 컬럼 이용하는 방안 강구할것.
            //선별급여와 마찬가지로 SLIP, 수가별 본인부담율이 틀린것에 대한 구분자 표시 본인부담율 구하는 방법 강구할것.
            if (clsPmpaType.TIT.FCode != "F014" && VB.Left(clsPmpaType.TIT.Bi, 1) == "1" && clsPmpaType.TIT.VCode.Trim() == "" && clsPmpaType.TIT.OgPdBun.Trim() == "")
            {

                SQL = "";
                SQL += ComNum.VBLF + "  SELECT a.QTY,SUM(a.AMT1) Amt ";
                SQL += ComNum.VBLF + "    From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                SQL += ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_SUN b ";
                SQL += ComNum.VBLF + "   Where a.TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SQL += ComNum.VBLF + "     AND a.SUNEXT = b.SUNEXT(+)  ";
                SQL += ComNum.VBLF + "     AND b.DTLBUN = '1100' ";
                SQL += ComNum.VBLF + "     AND a.QTY <= 0.9 ";
                SQL += ComNum.VBLF + "     AND a.QTY >= 0.85  ";
                SQL += ComNum.VBLF + "     AND a.SUNEXT not in ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B','AV820A','AV820B','AV8201A','AV8201B') ";
                SQL += ComNum.VBLF + "   GROUP By a.QTY ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        nAmt = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());
                        if (Convert.ToDouble(Dt.Rows[i]["QTY"].ToString()) == 0.9)
                        {
                            nAMT09 = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString()); //본인부담 25%
                        }
                        else
                        {
                            nAMT85 = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString()); //본인부담 30%
                        }

                        nTotGubyo -= nAmt;
                    }
                }
                Dt.Dispose();
                Dt = null;
            }
            #endregion

            #region 장기입원 대상자 입원료 본인부담 선별대상 구분 
            //입원료 본인부담율 선별구분되어짐으로 IPD_NEW_SLIP에 BonRate 컬럼 이용하는 방안 강구할것.
            //선별급여와 마찬가지로 SLIP, 수가별 본인부담율이 틀린것에 대한 구분자 표시 본인부담율 구하는 방법 강구할것.
            if (string.Compare(clsPmpaType.TIT.M_InDate, "2020-01-01") >= 0 && clsPmpaType.TIT.FCode != "F014" && VB.Left(clsPmpaType.TIT.Bi, 1) == "1" && clsPmpaType.TIT.VCode.Trim() == "" && clsPmpaType.TIT.OgPdBun.Trim() == "")
            {

                SQL = "";
                SQL += ComNum.VBLF + "  SELECT a.QTY,SUM(a.AMT1) Amt ";
                SQL += ComNum.VBLF + "    From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                SQL += ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_SUN b ";
                SQL += ComNum.VBLF + "   Where a.TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SQL += ComNum.VBLF + "     AND a.SUNEXT = b.SUNEXT(+)  ";
                SQL += ComNum.VBLF + "     AND b.DTLBUN = '1100' ";
                SQL += ComNum.VBLF + "     AND a.QTY <= 0.9 ";
                SQL += ComNum.VBLF + "     AND a.QTY >= 0.85  ";
                SQL += ComNum.VBLF + "     AND a.SUNEXT  in ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B') ";
                SQL += ComNum.VBLF + "   GROUP By a.QTY ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        nAmt = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());
                        if (Convert.ToDouble(Dt.Rows[i]["QTY"].ToString()) == 0.9)
                        {
                            nAMT09_H = (long)Math.Truncate(nAmt * 5 / 100.0);   //본인부담 5%가산   
                        }
                        else
                        {
                            nAMT85_H = (long)Math.Truncate(nAmt * 10 / 100.0);   //본인부담 10%가산   
                        }

                 
                    }
                }
                Dt.Dispose();
                Dt = null;
            }
            #endregion


            nCTMRAmt = 0;
            nCTMRBonin = 0;
            nBohoFood = 0;
            nFoodAmt = 0;
            nFoodGaAmt = 0;
            nFoodAmtBonin = 0;
            nFoodGaAmtBonin = 0;
            nFoodSumAmt = 0;
            nToothAmt = 0;
            nToothBonin = 0;
            nHRoomAmt = 0;
            nHRoomBonin = 0;
            nHUSetAmt = 0;
            nHUSetBonin = 0;

            //항결핵약제비
            clsPmpaPb.GnAntiTubeDrug_Amt = 0;
            clsPmpaPb.GnHRoomAmt = 0;
            clsPmpaPb.GnHRoomBonin = 0;
            clsPmpaPb.GnHUSetAmt = 0;
            clsPmpaPb.GnHUSetBonin = 0;

            long nAMT100 = 0;  // 의뢰회신서 100%급여 '2018 - 05 - 01
            long nTuberEduAmt = 0;  // 결핵관리료, 상담료 2021-09-16

            #region //입원 본인부담율 세팅 : 이전 루틴에서는 사용안함
            ////기준일자 세팅
            //cBON.SDATE = clsPmpaType.TIT.InDate;
            //strJuminNo = clsPmpaType.TIT.Jumin1 + clsPmpaType.TIT.Jumin3;

            ////나이구분 체크
            //cBON.IO = "I";
            //cBON.BI = clsPmpaType.TIT.Bi;
            //cBON.CHILD = cPF.Acct_Age_Gubun(clsPmpaType.TIT.Age, strJuminNo, strBDate);
            //if (clsPmpaType.TIT.OgPdBun == "1")
            //{
            //    cBON.MCODE = "E000";
            //    cBON.VCODE = "EV00";
            //}
            //else if (clsPmpaType.TIT.OgPdBun == "2")
            //{
            //    cBON.MCODE = "F000";
            //    cBON.VCODE = "EV00";
            //}
            //else
            //{
            //    cBON.VCODE = clsPmpaType.TIT.VCode;
            //    cBON.MCODE = cPF.Rtn_Ipd_OgPdBunDtl(clsPmpaType.TIT.OgPdBun);
            //}
            ////cBON.VCODE = clsPmpaType.TIT.VCode;
            ////cBON.MCODE = cPF.Rtn_Ipd_OgPdBunDtl(clsPmpaType.TIT.OgPdBun);
            //if (cPF.Rtn_Ipd_OgPdBunDtl(clsPmpaType.TIT.OgPdBun) == "")
            //{
            //    cBON.OGPDBUN = clsPmpaType.TIT.OgPdBun;
            //}
            //else
            //{
            //    cBON.OGPDBUN = clsPmpaType.TIT.OgPdBundtl;
            //}
            //cBON.DEPT = clsPmpaType.TIT.DeptCode;
            //cBON.FCODE = clsPmpaType.TIT.FCode;

            ////기본 부담율 계산
            //if (cIAcct.Read_IBon_Rate(pDbCon, cBON) == false)
            //{
            //    clsAlert cA = new ComPmpaLibB.clsAlert();
            //    cA.Alert_BonRate(cBON);
            //    return false;
            //}

            //if (clsPmpaType.TIT.Bohun == "3")   //장애인은 본인부담율 재조정 
            //{
            //    if ((cBON.BI == "11" && cBON.MCODE == "F000") || cBON.BI == "22")
            //    {
            //        clsPmpaType.IBR.Jin = 0;
            //        clsPmpaType.IBR.Bohum = 0;
            //        clsPmpaType.IBR.CTMRI = 0;
            //    }
            //}

            #endregion

            #region 식대계산
            SQL = "";
            SQL += ComNum.VBLF + " SELECT CASE                     ";
            SQL += ComNum.VBLF + "      WHEN TO_CHAR(BDATE,'YYYY-MM-DD') <= '2007-12-31' THEN '2007-12-31'";
            SQL += ComNum.VBLF + "      Else '2008-01-01'";
            SQL += ComNum.VBLF + "  END BDATE,";
            SQL += ComNum.VBLF + "  SUM(AMT1) FoodAmt ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                        ";
            SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTRSNO + "                                                    ";
            SQL += ComNum.VBLF + "    AND BUN IN ('74')                                                             ";
            SQL += ComNum.VBLF + "    AND NU  = '16'                                                                "; //식대 급여
            SQL += ComNum.VBLF + "    AND SUCODE NOT IN ('Y1110','T1110','Z4200','Z0100','Z0011','Z0010','Z0020')   ";
            SQL += ComNum.VBLF + "    AND SUBSTR(SUCODE, 1,1) NOT IN ('F')                                          "; //기존에 식대코드가 F로 시작한것 제외함."
            if (ArgTempTrans == "임시자격")
            {
                SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
            }
            else
            {
                if (ArgSDate != "")
                    SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
            }
            SQL += ComNum.VBLF + "  GROUP BY BDATE                                                                  ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
            nRead = Dt.Rows.Count;
            if (nRead > 0)
            {
                nFoodAmt = Convert.ToInt64(Dt.Rows[0]["FoodAmt"].ToString());

                for (i = 0; i < nRead; i++)
                {
                    strBBDate = Dt.Rows[i]["BDATE"].ToString().Trim();
                    if (string.Compare(strBBDate,  "2007-12-31") <= 0)
                    {
                        strBBDate = strBBDate;
                    }
                    nFoodAmt = Convert.ToInt64(Dt.Rows[i]["FoodAmt"].ToString());
                    nFoodSumAmt += nFoodAmt;

                    if (clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22")
                    {
                        if (VB.Trim(clsPmpaType.TIT.VCode) != "")
                        {
                            if (string.Compare(clsPmpaType.TIT.InDate, "2010-01-01") >= 0)
                            {
                                if (clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22")
                                {
                                    switch (VB.Trim(clsPmpaType.TIT.VCode))
                                    {
                                        case "V191":
                                        case "V192":
                                        case "V193":
                                        case "V194":
                                        case "V268":
                                        case "V275":
                                            nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * 5 / 100));
                                            clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * 5 / 100));
                                            break;
                                        case "V247":
                                        case "V248":
                                        case "V249":
                                        case "V250":
                                            if (string.Compare(clsPmpaType.TIT.InDate, "2010-07-01") >= 0)
                                            {
                                                nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * 5 / 100));
                                                clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * 5 / 100));
                                            }
                                            else
                                            {
                                                nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * 20 / 100));
                                                clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * 20 / 100));
                                            }
                                            break;
                                        default:
                                            nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * 20 / 100));
                                            clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * 20 / 100));
                                            break;
                                    }
                                }
                                else
                                {
                                    nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * clsPmpaType.TIT.BonRate / 100));
                                    clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * clsPmpaType.TIT.BonRate / 100));
                                }
                            }
                            else
                            {
                                if (clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22")
                                {
                                    switch (VB.Trim(clsPmpaType.TIT.VCode))
                                    {
                                        case "V191":
                                        case "V192":
                                        case "V193":
                                        case "V194":
                                        case "V268":
                                        case "V275":
                                            nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * 10 / 100));
                                            clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * 10 / 100));
                                            break;
                                        default:
                                            nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * 20 / 100));
                                            clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * 20 / 100));
                                            break;
                                    }
                                }
                                else
                                {
                                    nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * clsPmpaType.TIT.BonRate / 100));
                                    clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * clsPmpaType.TIT.BonRate / 100));
                                }
                            }
                        }
                        else if (clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22")
                        {
                            if (clsPmpaType.TIT.OgPdBun == "O" || clsPmpaType.TIT.Age < 6)
                            {
                                nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * clsPmpaType.TIT.BonRate / 100));
                                clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * clsPmpaType.TIT.BonRate / 100));
                            }
                            else
                            {
                                nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * 20 / 100));
                                clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * 20 / 100));
                            }
                        }
                        else
                        {
                            nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * clsPmpaType.TIT.BonRate / 100));
                            clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * clsPmpaType.TIT.BonRate / 100));
                        }
                    }
                    else if (string.Compare(strBBDate, "2007-12-31") <= 0)
                    {
                        nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * clsPmpaType.TIT.BonRate / 100));
                        clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * clsPmpaType.TIT.BonRate / 100));
                    }
                    else if (string.Compare(strBBDate, "2008-01-01") >= 0)
                    {
                        if (clsPmpaType.TIT.OgPdBun == "C")
                        {
                            nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * 20 / 100));
                            clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * 20 / 100));
                        }
                        else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && clsPmpaType.TIT.OgPdBun == "E")
                        {
                            nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * 20 / 100));
                            clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * 20 / 100));
                        }
                        else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && clsPmpaType.TIT.OgPdBun == "F")
                        {
                            nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * 20 / 100));
                            clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * 20 / 100));
                        }
                        else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && (clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2"))
                        {
                            nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * 20 / 100));
                            clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * 20 / 100));
                        }
                        else if (clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13")
                        {
                            nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * 50 / 100));
                            clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * 50 / 100));
                        }
                        else
                        {
                            nFoodAmtBonin = nFoodAmtBonin + (long)Math.Round((double)(nFoodAmt * clsPmpaType.TIT.BonRate / 100));
                            clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt5[3] + (long)Math.Round((double)(nFoodAmt * clsPmpaType.TIT.BonRate / 100));
                        }
                    }
                }
            }

            Dt.Dispose();
            Dt = null;

            #endregion

            #region 가산식대 계산
            SQL = "";
            SQL += ComNum.VBLF + " SELECT SUM(AMT1) FoodGaAmt ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
            SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTRSNO + " ";
            SQL += ComNum.VBLF + "    AND BUN IN ('74') ";
            SQL += ComNum.VBLF + "    AND NU  = '16' "; //급여 식대
            SQL += ComNum.VBLF + "    AND SUCODE  IN ('Y1110','T1110','Z4200','Z0100','Z0011','Z0010','Z0020') ";
            if (ArgTempTrans == "임시자격")
            {
                SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
            }
            else
            {
                if (ArgSDate != "")
                    SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
            }
            SQL += ComNum.VBLF + "  GROUP BY  BUN ";
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
            nRead = Dt.Rows.Count;
            if (nRead > 0)
            {
                if (clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "21")
                    ComFunc.MsgBox("의료급여 환자에 가산식대가 발생하였습니다. 식대수가를 변경해주세요.", "확인");

                nFoodGaAmt = Convert.ToInt64(Dt.Rows[0]["FoodGaAmt"].ToString());

                if (clsPmpaType.TIT.Bi == "51" || clsPmpaType.TIT.Bi == "43" || clsPmpaType.TIT.Bi == "55" || clsPmpaType.TIT.Bi == "41" || clsPmpaType.TIT.Bi == "42")
                {
                    nFoodGaAmtBonin = (long)Math.Truncate((nFoodGaAmt * 100 / 100) * 0.1) * 10;                     //식대가산금액
                    clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(((nFoodGaAmt * 100 / 100) * 0.1) * 10);
                }
                else if (clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "33" || clsPmpaType.TIT.Bi == "31")
                {
                    nFoodGaAmtBonin = (long)Math.Truncate(((nFoodGaAmt * (clsPmpaType.TIT.BonRate / 100.0)) * 0.1) * 10);           //식대가산금액
                    clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(((nFoodGaAmt * (clsPmpaType.TIT.BonRate / 100.0)) * 0.1) * 10);
                }
                else
                {
                    if (clsPmpaType.TIT.OgPdBun == "P")
                    { nFoodGaAmtBonin = 0; }                                    //식대가산금액
                    else if (clsPmpaType.TIT.OgPdBun == "C")             //차상위계층환자는 가산식대 없음
                    { nFoodGaAmtBonin = 0; }                                     //식대가산금액
                    else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && (clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2"))
                    { nFoodGaAmtBonin = 0; }                                        //식대가산금액
                    else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && clsPmpaType.TIT.OgPdBun == "E")   //차상위계층2 만성질환자는 가산식대 (전액청구) 본인0%'2009 - 04 - 01
                    { nFoodGaAmtBonin = 0; }                                      //식대가산금액
                    else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && clsPmpaType.TIT.OgPdBun == "F")   //차상위계층2 장애인 만성질환자는 가산식대(전액청구) 본인0%'2009 - 04 - 01
                    { nFoodGaAmtBonin = 0; }                                        //식대가산금액
                    else
                    {
                        nFoodGaAmtBonin = (long)Math.Truncate(nFoodGaAmt * 0.5);    //식대가산금액
                        clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(nFoodGaAmt * 50 / 100.0);
                    }
                }
            }
            Dt.Dispose();
            Dt = null;
            #endregion

            #region CT,MRI 보험,보호는 외래 부담율을 적용함
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,Bun,SUM(Amt1) CTMRIAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTRSNO + " ";
                SQL += ComNum.VBLF + "    AND BUN IN ('72','73') ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                SQL += ComNum.VBLF + "    AND GbSUGBS ='0' ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "  GROUP BY BDATE, BUN ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strBDate = Dt.Rows[i]["BDATE"].ToString().Trim();
                        strBun = Dt.Rows[i]["Bun"].ToString().Trim();
                        nAmt = Convert.ToInt64(Dt.Rows[i]["CTMRIAmt"].ToString());
                        nOpdBonRate = cPF.READ_OpdBonin_Rate_CHK(clsDB.DbCon, clsPmpaType.TIT.Bi, strBDate);
                        nCTMRAmt += nAmt;

                        if (clsPmpaType.TIT.OgPdBun == "C")
                        {
                            nBonRate = 0;  //차상위계층환자는 CT,MRI 본인부담율이 0%
                        }
                        else if (string.Compare(clsPmpaType.TIT.InDate, "2010-07-01") >= 0
                                && (clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22")
                                && (clsPmpaType.TIT.VCode == "V247" || clsPmpaType.TIT.VCode == "V248" || clsPmpaType.TIT.VCode == "V249" || clsPmpaType.TIT.VCode == "V250"))
                        {
                            nBonRate = 5;        //2010-07-01 중증화상 2010-07-01 5%
                        }
                        else if (string.Compare(clsPmpaType.TIT.InDate, "2009-12-01") >= 0
                                && (clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11")
                                && (clsPmpaType.TIT.VCode == "V193" || clsPmpaType.TIT.VCode == "V194"))
                        {
                            nBonRate = 5;        //2009-12-01 중증암 2009-12-01 5%
                        }
                        else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22")
                                && (clsPmpaType.TIT.VCode == "V000" || clsPmpaType.TIT.VCode == "V010"))  //V010(잠복결핵)은 V000과 동일하게(2021-06-28)(다른 스크립트 동일하게 적용)
                        {
                            nBonRate = 0;  //2016-07-01 V000,V010 결핵치료는 0%
                        }
                        else if (string.Compare(clsPmpaType.TIT.InDate, "2009-07-01") >= 0
                                && (clsPmpaType.TIT.OgPdBun == "V" || clsPmpaType.TIT.OgPdBun == "H") && clsPmpaType.TIT.Bi != "21")
                        {
                            nBonRate = 10;       //2009-07-01 등록희귀난치V, 희귀난치H 는 10%
                            if (clsPmpaType.TIT.VCode == "V000" || clsPmpaType.TIT.VCode == "V010")
                            {
                                nBonRate = 0; //2016-07-01 V000,V010 결핵치료는 0%
                            }
                        }
                        else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11")
                                && (clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "F" || clsPmpaType.TIT.OgPdBun == "J"
                                || clsPmpaType.TIT.OgPdBun == "K" || clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2"))
                        {
                            if (string.Compare(clsPmpaType.TIT.InDate, "2010-01-01") >= 0)
                            {
                                if (clsPmpaType.TIT.OgPdBundtl == "O" || clsPmpaType.TIT.OgPdBundtl == "S" || clsPmpaType.TIT.OgPdBundtl == "P")
                                {
                                    nBonRate = 0;        //자연분만산모,6세미만아동,신생아 ct.mri 본인부담 0% 2009-04-01
                                }
                                else if (clsPmpaType.TIT.OgPdBundtl == "Y" && (clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "F")
                                        && string.Compare(clsPmpaType.TIT.InDate, "2017-10-01") >= 0)
                                {
                                    nBonRate = 3;
                                }
                                else if (clsPmpaType.TIT.VCode == "V191" || clsPmpaType.TIT.VCode == "V192" || clsPmpaType.TIT.VCode == "V193"
                                        || clsPmpaType.TIT.VCode == "V194" || clsPmpaType.TIT.VCode == "V268" || clsPmpaType.TIT.VCode == "V275")
                                {
                                    nBonRate = 5;        //차상위2종-중증, ct.mri 본인부담 5%  2010-01-01
                                }
                                else if (clsPmpaType.TIT.DeptCode == "NP" && (clsPmpaType.TIT.OgPdBun != "J" && clsPmpaType.TIT.OgPdBun != "K"))
                                {
                                    nBonRate = 10;       //차상위2종-중증,정신과 ct.mri 본인부담 10%  2009-04-01
                                }
                                else if ((clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "F") && VB.UCase(VB.Left(clsPmpaType.TIT.VCode, 1)) == "V"
                                        && string.Compare(clsPmpaType.TIT.InDate, "2009-07-01") >= 0)
                                {
                                    nBonRate = 10;       //차상위 E,F 인데 희귀V 코드 있을경우 10% 2009-11-16 김준수샘 요청
                                }
                                else if ((clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2"))
                                {
                                    nBonRate = 10;
                                }
                                else
                                {
                                    nBonRate = 14;      //차상위2종 ct.mri 본인부담 14%  2009-04-01
                                }
                            }
                            else
                            {
                                if (clsPmpaType.TIT.OgPdBundtl == "O" || clsPmpaType.TIT.OgPdBundtl == "S" || clsPmpaType.TIT.OgPdBundtl == "P")
                                {
                                    nBonRate = 0;       //자연분만산모,6세미만아동,신생아 ct.mri 본인부담 0% 2009-04-01
                                }
                                else if (clsPmpaType.TIT.VCode == "V193" || clsPmpaType.TIT.DeptCode == "NP" && (clsPmpaType.TIT.OgPdBun != "J" && clsPmpaType.TIT.OgPdBun != "K"))
                                {
                                    nBonRate = 10;       //차상위2종-중증,정신과 ct.mri 본인부담 10%  2009-04-01
                                }
                                else if ((clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "F") && VB.UCase(VB.Left(clsPmpaType.TIT.VCode, 1)) == "V"
                                        && string.Compare(clsPmpaType.TIT.InDate, "2009-07-01") >= 0)
                                {
                                    nBonRate = 10;       //차상위 E,F 인데 희귀V 코드 있을경우 10% 2009-11-16 김준수샘 요청
                                }
                                else
                                {
                                    nBonRate = 14;      //차상위2종 ct.mri 본인부담 14%  2009-04-01
                                }
                            }
                        }
                        else if ((clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13")
                                && (clsPmpaType.TIT.OgPdBun == "S" || clsPmpaType.TIT.OgPdBun == "Y")
                                && string.Compare(clsPmpaType.TIT.InDate, "2017-10-01") >= 0)
                        {
                            nBonRate = 5;        //건강보험 소아 15세미만 CT.MRI 본인부담 5% 김순옥계장 요청
                        }
                        else if ((clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13") && clsPmpaType.TIT.OgPdBun == "S")
                        {
                            nBonRate = 10;        //건강보험 소아 6세미만 CT.MRI 본인부담 10% 김순옥계장 요청
                        }
                        else
                        {
                            if (clsPmpaType.TIT.Bi == "21")
                            {
                                nBonRate = 0;                  //의료급여 CT.MRI 본인부담 0% - 2009-03-03 윤조연 수정 기존잘못되어있었음 (5%->0%))- 심사과 김순옥샘
                            }
                            else if (clsPmpaType.TIT.Bi == "22")
                            {
                                if (string.Compare(clsPmpaType.TIT.InDate, "2010-01-01") >= 0
                                    && (clsPmpaType.TIT.VCode == "V191" || clsPmpaType.TIT.VCode == "V192" || clsPmpaType.TIT.VCode == "V193"
                                    || clsPmpaType.TIT.VCode == "V194" || clsPmpaType.TIT.VCode == "V268" || clsPmpaType.TIT.VCode == "V275"))
                                {
                                    if ((clsPmpaType.TIT.VCode == "V191" || clsPmpaType.TIT.VCode == "V192" || clsPmpaType.TIT.VCode == "V268" || clsPmpaType.TIT.VCode == "V275"))
                                    {
                                        nBonRate = 0;            //2017-01-10 KYO CT,MRI =>본인부담률이 0%
                                    }
                                    else
                                    {
                                        nBonRate = 5;            //2009-06-01 의료급여2종 중증 환자는 CT,MRI =>본인부담률이 5%
                                    }
                                }
                                else if (string.Compare(clsPmpaType.TIT.InDate, "2017-10-01") >= 0 && clsPmpaType.TIT.OgPdBun == "Y")
                                {
                                    nBonRate = 3;
                                }
                                else if (string.Compare(clsPmpaType.TIT.InDate, "2009-06-01") >= 0)
                                {
                                    nBonRate = 10;            //2009-06-01 의료급여2종 환자는 CT,MRI =>본인부담률이 10%
                                }
                                else
                                {
                                    if (clsPmpaType.TIT.VCode == "V191" || clsPmpaType.TIT.VCode == "V193" || clsPmpaType.TIT.VCode == "V268" || clsPmpaType.TIT.VCode == "V275")
                                    {
                                        nBonRate = 10;            //V191,V193환자는 CT,MRI =>본인부담률이 10%
                                    }
                                    else
                                    {
                                        nBonRate = nOpdBonRate;    //'나머지 자격은 외래본인부담율 다름.
                                    }
                                }
                            }
                            else
                            {
                                if (clsPmpaType.TIT.OgPdBun == "C")
                                {
                                    nBonRate = 0;         //차상위계층환자는 CT,MRI 본인부담율이 0% 2008-09-22일 수정함.
                                }
                                else if (string.Compare(clsPmpaType.TIT.InDate, "2010-01-01") >= 0
                                        && (clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13")
                                        && (clsPmpaType.TIT.VCode == "V191" || clsPmpaType.TIT.VCode == "V192" || clsPmpaType.TIT.VCode == "V193"
                                        || clsPmpaType.TIT.VCode == "V194" || clsPmpaType.TIT.VCode == "V268" || clsPmpaType.TIT.VCode == "V275"))
                                {
                                    nBonRate = 5;        //중증환자는 CT,MRI =>본인부담률이 5% 2010-01-01
                                }
                                else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && clsPmpaType.TIT.OgPdBun == "E")
                                {
                                    nBonRate = 14;       //차상위계층2 만성질환자는 CT,MRI 본인부담율이 14% 2009-04-01일 수정함.
                                }
                                else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && clsPmpaType.TIT.OgPdBun == "F")
                                {
                                    nBonRate = 14;       //차상위계층2 장애인 만성질환자는 CT,MRI 본인부담율이 14% 2009-04-01일 수정함.
                                }
                                else
                                {
                                    nBonRate = nOpdBonRate;   //나머지 자격은 외래본인부담율 다름.
                                }
                            }
                        }

                        nCTMRBonin = nCTMRBonin + (long)Math.Round((double)(nAmt * nBonRate / 100));

                        if (strBun == "72")
                        {
                            clsPmpaType.RPG.Amt5[17] = clsPmpaType.RPG.Amt5[17] + (long)Math.Round((double)(nAmt * nBonRate / 100));
                        }
                        else if (strBun == "73")
                        {
                            clsPmpaType.RPG.Amt5[18] = clsPmpaType.RPG.Amt5[18] + (long)Math.Round((double)(nAmt * nBonRate / 100));
                        }
                    }
                }
                Dt.Dispose();
                Dt = null;
            }

            if (clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22" )
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUM(AMT1) FoodAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTRSNO + " ";
                SQL += ComNum.VBLF + "    AND BUN IN ('74') ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0'  "; 
                SQL += ComNum.VBLF + "    AND SUCODE  = 'F02T' ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "  GROUP BY  BUN ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    nAmt = (long)VB.Val(Dt.Rows[i]["FoodAmt"].ToString().Trim());
                    nBohoFood = nBohoFood + nAmt;
                }
                Dt.Dispose();
                Dt = null;
            }
            
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
            {
                nHRoomAmt = 0;
                nHRoomBonin = 0;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,Bun,SUM(Amt1) HRoomAmt  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTRSNO + " ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0'  ";
                SQL += ComNum.VBLF + "    AND SUCode IN  ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820',   'AO280A','AO2801A','AO2801B','AO2801B','AV820A','AV820B','AV8201A','AV8201B') ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "  GROUP BY  BDATE, BUN ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {

                    strBDate = Dt.Rows[i]["BDate"].ToString().Trim();
                    nAmt = (long)VB.Val(Dt.Rows[i]["HRoomAmt"].ToString().Trim());
                    nHRoomAmt = nHRoomAmt + nAmt;
                    nHRoomBonin = nHRoomBonin + (long)Math.Round((double)(nAmt * 40 / 100));
                }

                nHRoomBonin = nHRoomBonin + nAMT09_H + nAMT85_H;

                Dt.Dispose();
                Dt = null;
            }
            #endregion

            #region //노인틀니 보험,보호 2012-07-03
            if (string.Compare(clsPmpaType.TIT.Bi , "30") < 0 && clsPmpaType.TIT.DeptCode == "DT" )
            {
                nToothAmt = 0;
                nToothBonin = 0;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUM(AMT1) ToothAmt ,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE ,BUN ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTRSNO + " ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0'  ";
                SQL += ComNum.VBLF + "    AND SUCode IN ( SELECT SUNEXT FROM BAS_SUN WHERE DTLBUN in ('4003','4004') )  ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "  GROUP BY  BDATE, BUN ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    strBDate = Dt.Rows[i]["BDate"].ToString().Trim();
                    nAmt = (long)VB.Val(Dt.Rows[i]["ToothAmt"].ToString().Trim());
                    nToothAmt = nToothAmt + nAmt;

                    if (clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13")
                    {
                        if (clsPmpaType.TIT.OgPdBun == "C")
                        {
                            nToothBonin = nToothBonin + (long)Math.Round((double)(nAmt * 20 / 100));
                        }
                        else if (clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "F")
                        {
                            nToothBonin = nToothBonin + (long)Math.Round((double)(nAmt * 30 / 100));
                        }
                        else
                        {
                            nToothBonin = nToothBonin + (long)Math.Round((double)(nAmt * 50 / 100));
                        }
                    }
                    else if (clsPmpaType.TIT.Bi == "21")
                    {
                        nToothBonin = nToothBonin + (long)Math.Round((double)(nAmt * 20 / 100));
                    }
                    else if (clsPmpaType.TIT.Bi == "22")
                    {
                        nToothBonin = nToothBonin + (long)Math.Round((double)(nAmt * 30 / 100));
                    }
                }
                Dt.Dispose();
                Dt = null;

                

                //노인틀니부분금액 빼주고 밑에계산후 마지막 본인 더해줌 2012-07-03
                clsPmpaType.RPG.Amt1[49] = clsPmpaType.RPG.Amt1[49] - nToothAmt;
            }
            #endregion //노인틀니 보험,보호 2012-07-03

            #region //TA환자 때문에 CT,MRI,SONO 변수에 저장함(비급여)
            if (clsPmpaType.TIT.Bi == "52")
            {
                nCTMRAmt = 0;

                SQL = "       SELECT SUM(Amt1) CTMRIAmt ";
                SQL = SQL + ComNum.VBLF + "   FROM IPD_NEW_SLIP A, BAS_SUN B ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.TRSNO=" + ArgTRSNO + " ";
                SQL = SQL + ComNum.VBLF + "    AND A.BUN IN ('71','72','73') ";
                SQL = SQL + ComNum.VBLF + "    AND A.GbSelf ='1' ";
                SQL = SQL + ComNum.VBLF + "    AND A.NU > '20' ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = B.SUNEXT(+) ";
                SQL = SQL + ComNum.VBLF + "    AND SUGBR = '1' ";
                
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    nAmt = (long)VB.Val(Dt.Rows[i]["CTMRIAmt"].ToString().Trim());
                    nCTMRAmt = nCTMRAmt + nAmt;
                }
                Dt.Dispose();
                Dt = null;

                //보철료
                SQL = "       SELECT SUM(Amt1) CTMRIAmt ";
                SQL = SQL + ComNum.VBLF + "   FROM IPD_NEW_SLIP ";
                SQL = SQL + ComNum.VBLF + "  WHERE TRSNO=" + ArgTRSNO + " ";
                SQL = SQL + ComNum.VBLF + "    AND BUN IN ('40') ";
                SQL = SQL + ComNum.VBLF + "    AND GbSelf ='0' ";// '2006-10-10 원무과장(김보미과장) 요청 변경함.
                SQL = SQL + ComNum.VBLF + "    AND NU > '20' ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
           

                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    nAmt = (long)VB.Val(Dt.Rows[i]["CTMRIAmt"].ToString().Trim());
                    nCTMRAmt = nCTMRAmt + nAmt;
                }
                Dt.Dispose();
                Dt = null;
            }
            #endregion //TA환자 때문에 CT,MRI,SONO 변수에 저장함(비급여)

            string strSugbs = "";

            #region //clsPmpaType.TIT.Bi < "30"
            if (string.Compare(clsPmpaType.TIT.Bi , "30") < 0)
            {
                nTot100Amt = 0;
                n100Amt = 0;

                //100/100 본인일부 부담금중 조합부담금액 계산(급여본인부담금액에서 제외됨) 2014-03-26
                SQL = "       SELECT SUM(Amt1) Amt,GbSuGbs ";
                SQL = SQL + ComNum.VBLF + "   FROM IPD_NEW_SLIP ";
                SQL = SQL + ComNum.VBLF + "  WHERE TRSNO=" + ArgTRSNO + " ";
                SQL = SQL + ComNum.VBLF + "    AND GBSUGBS IN ('4','5','9') ";
                SQL = SQL + ComNum.VBLF + "    AND GbSelf ='0' ";
                SQL = SQL + ComNum.VBLF + "    AND BDATE>=TO_DATE('2014-12-01','YYYY-MM-DD') ";//    '2014-12-01 부터 시행
                SQL = SQL + ComNum.VBLF + "  GROUP BY GbSuGbs ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    strSugbs = Dt.Rows[i]["GbSuGbs"].ToString().Trim();
                    nAmt = (long)VB.Val(Dt.Rows[i]["Amt"].ToString().Trim());
                    nTot100Amt = nTot100Amt + nAmt;

                    if (strSugbs == "4")
                        n100Amt = n100Amt + (long)Math.Round((double)(nAmt * 80 / 100));//      '본인부담 80%
                    else if (strSugbs == "5")
                        n100Amt = n100Amt + (long)Math.Round((double)(nAmt * 50 / 100));//      '본인부담 50%
                    else if (strSugbs == "9")
                        n100Amt = n100Amt + (long)Math.Round((double)(nAmt * 90 / 100));//      '본인부담 90%
                }
                Dt.Dispose();
                Dt = null;

                //100/80, 100/50 본인일부 부담금중 조합부담금액 계산(급여본인부담금액에서 제외됨) 2016-08-30
                SQL = "       SELECT /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1) Amt,GbSuGbs ";
                SQL = SQL + ComNum.VBLF + "   FROM IPD_NEW_SLIP ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + clsPmpaType.TIT.Pano + "' ";
                SQL = SQL + ComNum.VBLF + "    AND TRSNO=" + ArgTRSNO + " ";
                SQL = SQL + ComNum.VBLF + "    AND GBSUGBS IN ('3','6','7','8') ";//                    '2018-01-01 add
                SQL = SQL + ComNum.VBLF + "    AND GbSelf ='0' ";
                SQL = SQL + ComNum.VBLF + "    AND BDATE>=TO_DATE('2016-09-01','YYYY-MM-DD') ";//   '2016-09-01 부터 시행
                SQL = SQL + ComNum.VBLF + "  GROUP BY GbSuGbs ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    strSugbs = Dt.Rows[i]["GbSuGbs"].ToString().Trim();
                    nAmt = (long)VB.Val(Dt.Rows[i]["Amt"].ToString().Trim());
                    nTot100Amt = nTot100Amt + nAmt;

                    if (strSugbs == "6")
                        n100Amt = n100Amt + (long)Math.Round((double)(nAmt * 80 / 100));//    '본인부담 80%
                    else if (strSugbs == "3")
                        n100Amt = n100Amt + (long)Math.Round((double)(nAmt * 30 / 100));//      '본인부담 30%   
                    else if (strSugbs == "8" )
                        n100Amt = n100Amt + (long)Math.Round((double)(nAmt * 90 / 100));//      '본인부담 90%    2018-01-01 add
                    else if (strSugbs == "7" )
                        n100Amt = n100Amt + (long)Math.Round((double)(nAmt * 50 / 100));//      '본인부담 50%
                }
                Dt.Dispose();
                Dt = null;

            } //if (string.Compare(clsPmpaType.TIT.Bi , "30") < 0)

            #endregion //clsPmpaType.TIT.Bi < "30"

            #region //nAMT100의뢰회신서 100%급여 '2018-05-01
            if (clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,Bun,SUM(Amt1) AMT100  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTRSNO + " ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0'  "; 
                SQL += ComNum.VBLF + "    AND SUCODE  IN ( 'IA221','IA231' )  ";
                if (ArgTempTrans == "임시자격")
                {
                    SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                    SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
                }
                else
                {
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                }
                SQL += ComNum.VBLF + "  GROUP BY BDATE, BUN ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                nRead = Dt.Rows.Count;
                 
                for (i = 0; i < nRead; i++)
                {
                    nAmt = (long)VB.Val(Dt.Rows[i]["AMT100"].ToString().Trim());
                    nAMT100 = nAMT100 + nAmt;
                }
                Dt.Dispose();
                Dt = null;

                clsPmpaType.RPG.Amt1[1] = clsPmpaType.RPG.Amt1[1] - nAMT100;
            }
            #endregion //nAMT100의뢰회신서 100%급여 '2018-05-01


            #region //nTuberEduAmt 결핵관리료, 상담료 100%급여 '2021-09-16
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,Bun,SUM(Amt1) AMT100  ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
            SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTRSNO + " ";
            SQL += ComNum.VBLF + "    AND SUCODE  IN ( 'ID110','ID120','ID130' )  ";
            SQL += ComNum.VBLF + "    AND GbSelf ='0'  ";
            if (ArgTempTrans == "임시자격")
            {
                SQL += ComNum.VBLF + "   AND BDATE>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                SQL += ComNum.VBLF + "   AND BDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')     ";
            }
            else
            {
                if (ArgSDate != "")
                    SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
            }
            SQL += ComNum.VBLF + "  GROUP BY BDATE, BUN ";
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
            nRead = Dt.Rows.Count;
            for (i = 0; i < nRead; i++)
            {
                nAmt = (long)VB.Val(Dt.Rows[i]["AMT100"].ToString().Trim());
                nTuberEduAmt = nTuberEduAmt + nAmt;
            }
            Dt.Dispose();
            Dt = null;

            clsPmpaType.RPG.Amt1[1] = clsPmpaType.RPG.Amt1[1] - nTuberEduAmt;
            
            #endregion 


            #region //격리병실료 산정        '2016-09-22
            nKekliAmt = 0;
            nKekliAmt_Bon = 0;
            nAmt = 0;

            if (string.Compare(clsPmpaType.TIT.Bi , "30") < 0)
            {
                SQL = "       SELECT /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1) KekliAmt ";
                SQL = SQL + ComNum.VBLF + "   FROM IPD_NEW_SLIP ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + clsPmpaType.TIT.Pano + "' ";
                SQL = SQL + ComNum.VBLF + "    AND TRSNO=" + ArgTRSNO + " ";
                SQL = SQL + ComNum.VBLF + "    AND SUCode IN ('AJ010','AJ020','AK200','AK201','AK202','AK210','AK211','V6001','V6002','AK200A','AH001','AH002') ";// 'AK200A  2017-07-18 ADD
                SQL = SQL + ComNum.VBLF + "    AND BDate>=TO_DATE('2016-09-23','YYYY-MM-DD') ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                for (i = 0; i < nRead; i++)
                {
                    nAmt = (long)VB.Val(Dt.Rows[i]["KekliAmt"].ToString().Trim());
                    nKekliAmt = nKekliAmt + nAmt;

                    if (clsPmpaType.TIT.VCode != "")
                    {
                        if (clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13")
                        {
                            if (clsPmpaType.TIT.VCode == "V000" || clsPmpaType.TIT.VCode == "V010")
                            {
                                nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * 0 / 100));
                            }
                            else
                            {
                                nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * clsPmpaType.IBR.Bohum / 100.0));
                            }
                        }
                    }
                    else
                    {
                        if (clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13")
                        {
                            //F자격 제외 실제로는 본인부담없음
                            if (clsPmpaType.TIT.OgPdBun == "E"  && clsPmpaType.TIT.Age < 6)
                            {
                                //nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * 0 / 100));  //0은 계산안됨
                            }
                            else if (clsPmpaType.TIT.OgPdBun == "E" && clsPmpaType.TIT.Age <= 15 )
                            {
                                nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * 5 / 100));
                            }
                            else if (clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2" || clsPmpaType.TIT.OgPdBun == "V" || clsPmpaType.TIT.OgPdBun == "H" || clsPmpaType.TIT.OgPdBun == "S" || clsPmpaType.TIT.OgPdBun == "Y")
                            {
                                nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * 5 / 100));
                            }
                            else if (clsPmpaType.TIT.OgPdBun == "F" || clsPmpaType.TIT.OgPdBun == "C" || clsPmpaType.TIT.OgPdBun == "P")
                            {
                                //nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * 0 / 100));  //0은 계산안됨
                            }
                            else
                            {
                                nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * 10 / 100));
                            }
                        }
                        else if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.OgPdBun == "P")
                        {
                            //    nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * 0 / 100)); //0은 계산안됨
                        }
                        else if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.OgPdBun == "Y")
                        {
                            nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * 3 / 100));
                        }
                        else if (clsPmpaType.TIT.Bi == "22")
                        {
                            //2021-06-17 의뢰서 
                            nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * 10 / 100));
                            //nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * 5 / 100));
                        }
                    }
                }
                Dt.Dispose();
                Dt = null;

            }
            #endregion //격리병실료 산정        '2016-09-22

            //2014-06-07 항결핵약제비 계산(지원금에 포함)
            clsPmpaPb.GnAntiTubeDrug_Amt = cISC.Gesan_AntiTubeDrug_Amt(pDbCon, ArgTRSNO);

            //본인부담액을 계산함

            #region //급여 본인부담액을 계산
            switch (clsPmpaType.TIT.VCode)
            {
                case "V191":
                case "V192":
                case "V193":
                case "V194":
                case "V247":
                case "V248":
                case "V249":
                case "V250":
                case "V268":
                case "V275":
                    #region //중증환자 기존 + 중증화상 : "V191", "V192", "V193", "V194", "V247", "V248", "V249", "V250", "V268", "V275" 
                    switch (clsPmpaType.TIT.VCode)
                    {
                        case "V191":
                        case "V192":
                        case "V193":
                        case "V194":
                        case "V268":
                        case "V275":
                            #region //중증암 "V191", "V192", "V193", "V194", "V268", "V275"
                            if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && (clsPmpaType.TIT.OgPdBun == "V" || clsPmpaType.TIT.OgPdBun == "H"))
                            {
                                if (string.Compare(clsPmpaType.TIT.InDate, "2010-01-01") >= 0 && (clsPmpaType.TIT.VCode == "V191" || clsPmpaType.TIT.VCode == "V192" || clsPmpaType.TIT.VCode == "V193" || clsPmpaType.TIT.VCode == "V194" || clsPmpaType.TIT.VCode == "V268" || clsPmpaType.TIT.VCode == "V275"))
                                {
                                    nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 5 / 100); //2010-01-01 중증암 5%
                                    nBonGubyo = nBonGubyo + nFoodAmtBonin + nFoodGaAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                    nBonRate = 5;
                                }
                                else if (string.Compare(clsPmpaType.TIT.InDate, "2010-01-01") >= 0 && (clsPmpaType.TIT.VCode == "V193" || clsPmpaType.TIT.VCode == "V194"))
                                {
                                    nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 5 / 100); //'2009-12-01 중증암 5%
                                    nBonGubyo = nBonGubyo + nFoodAmtBonin + nFoodGaAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                    nBonRate = 5;
                                }
                                else
                                {
                                    nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 10 / 100); //'2009-07-01 희귀난치는 10%
                                    nBonGubyo = nBonGubyo + nFoodAmtBonin + nFoodGaAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                    nBonRate = 10;
                                }
                                Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, nBonRate, "");
                            }
                            else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && (clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "F" || clsPmpaType.TIT.OgPdBun == "J" || clsPmpaType.TIT.OgPdBun == "K" || clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2"))
                            {
                                if ((clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "F") && (clsPmpaType.TIT.VCode == "V191" || clsPmpaType.TIT.VCode == "V192" || clsPmpaType.TIT.VCode == "V268" || clsPmpaType.TIT.VCode == "V275") )
                                {
                                    nBonGubyo = 0;
                                    nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                                }
                                else if (clsPmpaType.TIT.OgPdBun == "F")
                                {
                                    nBonGubyo = 0;
                                    nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                                }
                                else if (clsPmpaType.TIT.OgPdBun == "2")
                                {
                                    nBonGubyo = 0;
                                    nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                                }
                                else if ((clsPmpaType.TIT.OgPdBundtl == "O" || clsPmpaType.TIT.OgPdBundtl == "S" || clsPmpaType.TIT.OgPdBundtl == "P") )
                                {
                                    nBonGubyo = 0;//          '중증+자연분만,6세미만 ,장애인
                                    nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                                }
                                else
                                {
                                    //2015-04-06 V268 뇌출혈추가
                                    if (string.Compare(clsPmpaType.TIT.InDate , "2010-01-01") >= 0 && (clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && (clsPmpaType.TIT.VCode == "V191" || clsPmpaType.TIT.VCode == "V192" || clsPmpaType.TIT.VCode == "V193" || clsPmpaType.TIT.VCode == "V194" || clsPmpaType.TIT.VCode == "V268" || clsPmpaType.TIT.VCode == "V275"))
                                    {
                                        nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 5 / 100);// '중증암 5%
                                        nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                        Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 5, "");
                                    }
                                    else if (string.Compare(clsPmpaType.TIT.InDate , "2009-12-01") >= 0 && (clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && (clsPmpaType.TIT.VCode == "V193" || clsPmpaType.TIT.VCode == "V194"))
                                    {
                                        nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 5 / 100);// '중증암 5%
                                        nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                        Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 5, "");
                                    }
                                    else
                                    {
                                        nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 10 / 100);
                                        nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                        Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 10, "");
                                    }
                                }
                            }
                            else
                            {
                                nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * clsPmpaType.TIT.BonRate / 100);
                                //2017-01-09 22종 중증 부담율 누락으로 추가함 KMC
                                if (clsPmpaType.TIT.Bi == "22" && (clsPmpaType.TIT.VCode == "V191" || clsPmpaType.TIT.VCode == "V192" || clsPmpaType.TIT.VCode == "V268" || clsPmpaType.TIT.VCode == "V275"))
                                {
                                    nBonGubyo = nFoodAmtBonin;//      '의료급여 2종 V191, V192는 식대 5%만 계산함 2013-12-20
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "");
                                }
                                else if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.Bohun != "3" && clsPmpaType.TIT.OgPdBun != "0" && clsPmpaType.TIT.OgPdBun != "P")
                                {
                                    nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, clsPmpaType.TIT.BonRate, "");
                                }
                                else if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22" || clsPmpaType.TIT.Bi == "24") && (clsPmpaType.TIT.OgPdBun == "O" || clsPmpaType.TIT.OgPdBun == "P"))
                                {
                                    nBonGubyo = 0;//                                                  '의료급여는 정상분만, 6세미만은 본인부담금 0
                                    nBonGubyo = nBonGubyo + nHRoomBonin;
                                    //clsPmpaType.RPG.Amt5[3] = clsPmpaType.RPG.Amt1[3] * 0 / 100;  //의료급여 소아 - 식대면제
                                    clsPmpaType.RPG.Amt5[3] = 0;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                                }
                                else if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.Bohun == "3" )
                                {
                                    nBonGubyo = 0;
                                    nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                                }
                                else if (clsPmpaType.TIT.Bi == "24")
                                {
                                    nBonGubyo = 0;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                                }
                                else
                                {
                                    nBonGubyo = nBonGubyo + nFoodAmtBonin + nFoodGaAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, clsPmpaType.TIT.BonRate, "");
                                }
                            }
                            #endregion //중증암 "V191", "V192", "V193", "V194", "V268", "V275"
                            break;
                        case "V247":
                        case "V248":
                        case "V249":
                        case "V250":
                            #region //중증화상 "V247", "V248", "V249", "V250"
                            if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && string.Compare (clsPmpaType.TIT.InDate , "2010-07-01") >= 0 )
                            {
                                nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 5 / 100);
                                nBonGubyo = nBonGubyo + nFoodAmtBonin + nFoodGaAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 5, "");
                            }
                            else if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && string.Compare(clsPmpaType.TIT.InDate, "2010-07-01") >= 0)
                            {
                                nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * clsPmpaType.TIT.BonRate / 100);

                                if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.Bohun != "3" && clsPmpaType.TIT.OgPdBun != "0" && clsPmpaType.TIT.OgPdBun != "P" )
                                {
                                    nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, clsPmpaType.TIT.BonRate, "");
                                }
                                else if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22" || clsPmpaType.TIT.Bi == "24") && (clsPmpaType.TIT.OgPdBun == "O" || clsPmpaType.TIT.OgPdBun == "P"))
                                {
                                    nBonGubyo = 0;
                                    nBonGubyo = nBonGubyo + nHRoomBonin;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                                }
                                else if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.Bohun == "3")
                                {
                                    nBonGubyo = 0;
                                    nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                                }
                                else if (clsPmpaType.TIT.Bi == "24")
                                {
                                    nBonGubyo = 0;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                                }
                                else
                                {
                                    nBonGubyo = nBonGubyo + nFoodAmtBonin + nFoodGaAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin; 
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, clsPmpaType.TIT.BonRate, "");
                                }
                            }
                            else
                            {
                                nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * clsPmpaType.TIT.BonRate / 100);
                                if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.Bohun != "3" && clsPmpaType.TIT.OgPdBun != "0" && clsPmpaType.TIT.OgPdBun != "P")
                                {
                                    nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, clsPmpaType.TIT.BonRate, "");
                                }
                                else if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22" || clsPmpaType.TIT.Bi == "24") && (clsPmpaType.TIT.OgPdBun == "O" || clsPmpaType.TIT.OgPdBun == "P"))
                                {
                                    nBonGubyo = 0;
                                    nBonGubyo = nBonGubyo + nHRoomBonin;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                                }
                                else if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.Bohun == "3")
                                {
                                    nBonGubyo = 0;
                                    nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                                }
                                else if (clsPmpaType.TIT.Bi == "24")
                                {
                                    nBonGubyo = 0;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                                }
                                else
                                {
                                    nBonGubyo = nBonGubyo + nFoodAmtBonin + nFoodGaAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                    Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, clsPmpaType.TIT.BonRate, "");
                                }
                            }
                            #endregion //중증화상 "V247", "V248", "V249", "V250"
                            break;
                    }

                    GnTempAmt_Bon = 0;
                    GnTempHRoomBonin = 0;// '상한제 2인실 제외 사용 2018-07-01


                    nBonGubyo = nBonGubyo + (long)Math.Round((double)(nAMT09 * 25 / 100));
                    nBonGubyo = nBonGubyo + (long)Math.Round((double)(nAMT85 * 30 / 100));

                    if (string.Compare(clsPmpaType.TIT.Bi , "20") <= 0 && ArgSDate == "" && ArgTempTrans != "임시자격")
                    {
                        GnTempAmt_Bon = nBonGubyo;
                        GnTempHRoomBonin = nHRoomBonin;

                        if (string.Compare(clsPmpaType.TIT.InDate , "2011-01-01") >= 0)
                        {
                            nBonGubyo = Ipd_Tewon_PrtAmt_Gesabn_Limit(clsDB.DbCon, "2", ref GnTempAmt_Bon, ref GnTempHRoomBonin); //2013-015
                        }
                        else
                        {
                            nBonGubyo = Ipd_Tewon_PrtAmt_Gesabn_Limit(clsDB.DbCon, "1", ref GnTempAmt_Bon, ref GnTempHRoomBonin);  //2013-015
                        }
                    }
                    #endregion //중증환자 기존 + 중증화상 : "V191", "V192", "V193", "V194", "V247", "V248", "V249", "V250", "V268", "V275" 
                    break;
                default:
                    #region //중증환자아닌 환자
                    if (string.Compare(clsPmpaType.TIT.InDate , "2009-07-01") >= 0 && (clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && (clsPmpaType.TIT.OgPdBun == "V" || clsPmpaType.TIT.OgPdBun == "H"))
                    {
                        if ((clsPmpaType.TIT.VCode == "V000" || clsPmpaType.TIT.VCode == "V010") && clsPmpaType.TIT.FCode != "F013")
                        {
                            nBonRate = 0;
                        }
                        else if (clsPmpaType.TIT.FCode == "F013")
                        {
                            nBonRate = 5;
                        }
                        else
                        {
                            nBonRate = 10;
                        }
                        nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * nBonRate / 100);
                        nBonGubyo = nBonGubyo + nFoodAmtBonin + nFoodGaAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                        Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, nBonRate, "");
                    }
                    else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && (clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "F" || clsPmpaType.TIT.OgPdBun == "J" || clsPmpaType.TIT.OgPdBun == "K" || clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2"))
                    {
                        if (clsPmpaType.TIT.OgPdBun == "F")
                        {
                            nBonGubyo = 0;
                            nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nHRoomBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                        }
                        else if (clsPmpaType.TIT.OgPdBun == "2")
                        {
                            nBonGubyo = 0;
                            nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                        }
                        else if ((clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "E") && string.Compare(clsPmpaType.TIT.InDate, "2017-10-01") >= 0 && clsPmpaType.TIT.OgPdBundtl == "Y")
                        {
                            nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 3 / 100);
                            nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 3, "1");
                        }
                        else if ((clsPmpaType.TIT.OgPdBundtl == "O" || clsPmpaType.TIT.OgPdBundtl == "S" || clsPmpaType.TIT.OgPdBundtl == "P"))
                        {
                            nBonGubyo = 0;//          '중증+자연분만,6세미만,장애인
                            nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                        }
                        else if (clsPmpaType.TIT.FCode == "F013")
                        {
                            nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 5 / 100);
                            nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 5, "1");
                        }
                        else if (clsPmpaType.TIT.DeptCode == "NP")
                        {
                            nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 10 / 100);
                            nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 10, "");
                        }
                        else if (clsPmpaType.TIT.OgPdBun == "E" && VB.UCase(VB.Left(clsPmpaType.TIT.VCode, 1)) == "V" && string.Compare(clsPmpaType.TIT.InDate, "2009-07-01") >= 0)
                        {
                            nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 10 / 100);// '2009-11-16 10%
                            nBonGubyo = nBonGubyo + nFoodAmtBonin + nFoodGaAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 10, "");
                        }
                        else if (clsPmpaType.TIT.OgPdBun == "1")
                        {
                            //2016-08-11 차상위E+V 중에 V000은 0%
                            if (clsPmpaType.TIT.VCode == "V000" || clsPmpaType.TIT.VCode == "V010")
                            {
                                //nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 0 / 100); //2009-11-16 10%
                                nBonGubyo = 0;
                                nBonGubyo = nBonGubyo + nFoodAmtBonin + nFoodGaAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "");
                            }
                            else
                            {
                                nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 10 / 100); //2009-11-16 10%
                                nBonGubyo = nBonGubyo + nFoodAmtBonin + nFoodGaAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                                Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 10, "");
                            }
                        }
                        else
                        {
                            nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 14 / 100);
                            nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 14, "");

                            clsPmpaType.RPG.Amt5[17] = (clsPmpaType.RPG.Amt1[17] * clsPmpaType.TIT.BonRate / 100);
                            clsPmpaType.RPG.Amt5[18] = (clsPmpaType.RPG.Amt1[18] * clsPmpaType.TIT.BonRate / 100); //MR,CT
                        }
                    }
                    else
                    {
                        if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && (clsPmpaType.TIT.VCode == "V000" || clsPmpaType.TIT.VCode == "V010"))
                        {
                            //nBonGubyo = ((nTotGubyo - nCTMRAmt - nBohoFood - nFoodSumAmt - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 0 / 100);
                            nBonGubyo = 0;
                        }
                        else if ((clsPmpaType.TIT.Bi != "21" && clsPmpaType.TIT.Bi != "22") && clsPmpaType.TIT.FCode == "F013")
                        {
                            nBonGubyo = ((nTotGubyo - nCTMRAmt - nBohoFood - nFoodSumAmt - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 5 / 100);
                        }
                        else
                        {
                            nBonGubyo = ((nTotGubyo - nCTMRAmt - nBohoFood - nFoodSumAmt - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * clsPmpaType.TIT.BonRate / 100);
                        }
                        //2016-07-04
                        if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.FCode == "F013")
                        {
                            nBonGubyo = nFoodAmtBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "");
                        }
                        else if (string.Compare(clsPmpaType.TIT.Bi , "13") <= 0 && clsPmpaType.TIT.FCode == "F013")
                        {
                            nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 5 / 100);
                            nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 5, "1");
                        }
                        else if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.OgPdBun == "T")
                        {
                            nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 5 / 100);
                            nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 5, "1");
                        }
                        else if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.OgPdBun == "Y" && string.Compare(clsPmpaType.TIT.InDate , "2017-10-01") >= 0)
                        {
                            nBonGubyo = ((nTotGubyo - nFoodSumAmt - nBohoFood - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt) * 3 / 100);
                            nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 3, "1");
                        }
                        else if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.Bohun != "3" && clsPmpaType.TIT.OgPdBun != "0" && clsPmpaType.TIT.OgPdBun != "P")
                        {
                            nBonGubyo = nBonGubyo + nCTMRBonin + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, clsPmpaType.TIT.BonRate, "");
                        }
                        else if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22" || clsPmpaType.TIT.Bi == "24") && (clsPmpaType.TIT.OgPdBun == "O" || clsPmpaType.TIT.OgPdBun == "P"))
                        {
                            nBonGubyo = 0;
                            nBonGubyo = nBonGubyo + nHRoomBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                        }
                        else if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.Bohun == "3")
                        {
                            nBonGubyo = 0;
                            nBonGubyo = nBonGubyo + nFoodAmtBonin + nToothBonin + nHRoomBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                        }
                        else if (clsPmpaType.TIT.Bi == "24")
                        {
                            nBonGubyo = 0;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                        }
                        else if (string.Compare(clsPmpaType.TIT.Bi , "13") <= 0 && (clsPmpaType.TIT.OgPdBun == "O" || clsPmpaType.TIT.OgPdBun == "P"))
                        {
                            nBonGubyo = 0;
                            nBonGubyo = nBonGubyo + nFoodGaAmtBonin + nFoodAmtBonin + nToothBonin + nHRoomBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, 0, "1");
                        }
                        else if (clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "31" || clsPmpaType.TIT.Bi == "32")
                        {
                            nBonGubyo = nBonGubyo + nCTMRBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                            clsPmpaType.RPG.Amt5[3] = (clsPmpaType.RPG.Amt1[3] * clsPmpaType.TIT.BonRate / 100); //산재 식대 0% 2012-01-02
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, clsPmpaType.TIT.BonRate, "");
                        }
                        else if (clsPmpaType.TIT.Bi == "33" || clsPmpaType.TIT.Bi == "55")
                        {
                            nBonGubyo = nBonGubyo + nCTMRBonin + nFoodAmtBonin + nFoodGaAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, clsPmpaType.TIT.BonRate, "1");
                            clsPmpaType.RPG.Amt5[22] = (clsPmpaType.RPG.Amt1[22] * clsPmpaType.TIT.BonRate / 100);
                        }
                        else if (string.Compare(clsPmpaType.TIT.Bi , "13") <= 0 && clsPmpaType.TIT.OgPdBun == "Y")
                        {
                            nBonGubyo = nBonGubyo + nCTMRBonin + nFoodGaAmtBonin + nFoodAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, clsPmpaType.TIT.BonRate, "1");
                        }
                        else
                        {
                            nBonGubyo = nBonGubyo + nCTMRBonin + nFoodAmtBonin + nFoodGaAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                            Ipd_RPG_Amt_Set_Junsan(clsDB.DbCon, clsPmpaType.TIT.BonRate, "");

                            if (clsPmpaType.TIT.OgPdBun == "C")
                            {
                            }
                            else if (string.Compare(clsPmpaType.TIT.Bi , "31") <= 0 && clsPmpaType.TIT.OgPdBun == "S" && string.Compare(clsPmpaType.TIT.InDate ,"2017-10-01") < 0)
                            {
                                clsPmpaType.RPG.Amt5[17] = (clsPmpaType.RPG.Amt1[17] * 10 / 100);
                                clsPmpaType.RPG.Amt5[18] = (clsPmpaType.RPG.Amt1[18] * 10 / 100); //MR,CT 2012-02-21
                            }
                            else if (string.Compare(clsPmpaType.TIT.Bi , "31") <= 0 && clsPmpaType.TIT.OgPdBun == "S" && string.Compare(clsPmpaType.TIT.InDate , "2017-10-01") >= 0)
                            {
                                clsPmpaType.RPG.Amt5[17] = (clsPmpaType.RPG.Amt1[17] * 5 / 100);
                                clsPmpaType.RPG.Amt5[18] = (clsPmpaType.RPG.Amt1[18] * 5 / 100); //MR,CT 2017-10-01
                            }
                            else
                            {
                                if ((clsPmpaType.TIT.Bi == "43" || clsPmpaType.TIT.Bi == "51") && nOpdBonRate == 0)
                                {
                                    nOpdBonRate = 100;
                                }
                                clsPmpaType.RPG.Amt5[17] = (clsPmpaType.RPG.Amt1[17] * nOpdBonRate / 100);
                                clsPmpaType.RPG.Amt5[18] = (clsPmpaType.RPG.Amt1[18] * nOpdBonRate / 100); // MR,CT
                            }
                        }

                        //본인부담 상한제
                        //재원기간동안 급여 본인부담액이 300만원을 초과하면 300만원만 본인부담, 나머지는 조합청구
                        GnTempAmt_Bon = 0;
                        GnTempHRoomBonin = nHRoomBonin;// '2018-07-01 add
                        nBonGubyo = nBonGubyo + (nAMT09 * 25 / 100);
                        nBonGubyo = nBonGubyo + (nAMT85 * 30 / 100);

                        if (string.Compare(clsPmpaType.TIT.Bi , "20") <= 0 && ArgSDate == "" && ArgTempTrans != "임시자격")
                        {
                            GnTempAmt_Bon = nBonGubyo;
                            if (string.Compare(clsPmpaType.TIT.InDate, "2011-01-01") >= 0)
                            {
                                nBonGubyo = Ipd_Tewon_PrtAmt_Gesabn_Limit(clsDB.DbCon, "2", ref GnTempAmt_Bon, ref GnTempHRoomBonin); //2013-015
                            }
                            else
                            {
                                nBonGubyo = Ipd_Tewon_PrtAmt_Gesabn_Limit(clsDB.DbCon, "1", ref GnTempAmt_Bon, ref GnTempHRoomBonin);  //2013-015
                            }
                        }

                    }
                    #endregion //중증환자아닌 환자
                    break;
            }

            #endregion //급여 본인부담액을 계산

            //노인틀니부분금액 +2인병실료 빼주고 밑에계산후 마지막 본인 더해줌 2012-07-03
            clsPmpaType.RPG.Amt1[49] = clsPmpaType.RPG.Amt1[49] + nToothAmt;
            clsPmpaType.RPG.Amt5[49] = clsPmpaType.RPG.Amt5[49] + nToothBonin;

            //clsPmpaType.RPG.Amt1[1] = clsPmpaType.RPG.Amt1[1] + nAMT100; //의뢰 회신사업 급여100% 2018-05-01 add
            clsPmpaType.RPG.Amt1[1] = clsPmpaType.RPG.Amt1[1] + nAMT100 + nTuberEduAmt; //의뢰 회신사업 급여100% 2018-05-01 add  //2021-09-16 결핵상담료, 관리료
            

            //비급여 본인부담액을 계산

            switch (clsPmpaType.TIT.Bi)
            {
                case "52":
                    nBonBiGubyo = nTotBiGubyo - nCTMRAmt - clsPmpaType.TIT.Amt[36] - clsPmpaType.TIT.Amt[37] - clsPmpaType.TIT.Amt[38] - clsPmpaType.TIT.Amt[39] - clsPmpaType.TIT.Amt[44];
                    nBoninAmt = nBonGubyo + nBonBiGubyo;
                    break;
                default:
                    nBonBiGubyo = nTotBiGubyo;
                    nBoninAmt = nBonGubyo + nBonBiGubyo + n100Amt;
                    break;
            }

            for (i = 1; i <= 22; i++)
            {
                clsPmpaType.RPG.Amt6[i] = clsPmpaType.RPG.Amt1[i] - clsPmpaType.RPG.Amt5[i];
            }

            clsPmpaType.RPG.Amt6[49] = clsPmpaType.RPG.Amt1[49] - clsPmpaType.RPG.Amt5[49]; //기타급여

            for (i = 1; i <= 49; i++)
            {
                clsPmpaType.RPG.Amt5[50] = clsPmpaType.RPG.Amt5[50] + clsPmpaType.RPG.Amt5[i];
                clsPmpaType.RPG.Amt6[50] = clsPmpaType.RPG.Amt6[50] + clsPmpaType.RPG.Amt6[i];
            }
            clsPmpaType.TIT.Amt[51] = 0;
            clsPmpaType.TIT.Amt[52] = 0;

            //약제상한금액 급여총액- 2011-03-30
            if (string.Compare(clsPublic.GstrSysDate ,"2011-04-01") >= 0 && clsPmpaType.TIT.Amt[64] != 0)
            {
                clsPmpaType.TIT.Amt[53] = (clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64]) - nBoninAmt; //조합부담금
            }
            else
            {
                clsPmpaType.TIT.Amt[53] = clsPmpaType.TIT.Amt[50] - nBoninAmt;  //조합부담금
            }

            //할인금액을 계산함
            clsPmpaType.TIT.Amt[54] = 0;
            clsPmpaType.TIT.Amt[55] = nBoninAmt;    //차인납부액

            if ((clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13") && clsPmpaType.TIT.OgPdBun == "F")
            {
                ////
            }

            if (string.Compare(clsPmpaType.TIT.GbGameK , "00") > 0)
            {
                if (clsPmpaType.TIT.OutDate.Trim() == "")
                {
                    if (clsPmpaType.TIT.GbGameK == "55" && clsPmpaType.TIT.GelCode == "")
                    {
                        ComFunc.MsgBox("계약처 감액인데 계약처 코드가 없습니다..!!!" + ComNum.VBLF + "등록번호 : " + clsPmpaType.TIT.Pano + "  진료과 : " + clsPmpaType.TIT.DeptCode);
                    }

                    if (cIAcct.IPD_Gamek_Account_Main(clsDB.DbCon,clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPublic.GstrSysDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt, nCTMRBonin) == false)
                    {
                        cPF = null;
                        return false;
                    }
                }
                else
                {
                    if (cIAcct.IPD_Gamek_Account_Main(clsDB.DbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPmpaType.TIT.OutDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt, nCTMRBonin) == false)
                    {
                        cPF = null;
                        return false;
                    }
                }
                clsPmpaType.TIT.Amt[54] = clsPmpaType.GAM.Halin_Tot;
                clsPmpaType.TIT.DtGamek = clsPmpaType.GAM.DTHalin_Tot;
                clsPmpaType.TIT.Amt[55] = nBoninAmt - clsPmpaType.GAM.Halin_Tot;
            }

            //인쇄용 계산은 저장 제외함
            cPF = null;

            return true;
        }

        /// <summary>
        /// Ipd_Tewon_PrtAmt_Gesabn_상한제
        /// Author : 박웅규
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pTempAmt_Bon"></param>
        /// <param name="pTempHRoomBonin"></param>
        /// <seealso cref="Report_Print2.bas : Ipd_Tewon_PrtAmt_Gesabn_상한제"/>
        private long Ipd_Tewon_PrtAmt_Gesabn_Limit(PsmhDb pDbCon, string ArgGubun, ref long GnTempAmt_Bon, ref long GnTempHRoomBonin)
        {
            DataTable dt = null;
            DataTable dtSub = null;
            DataTable dtSub2 = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int i = 0;
            int J = 0;
            long nBonGubyo = 0;
            string[] SangFDate = new string[21];
            string[] SangTDate = new string[21];
            string[] SangFDate_New = new string[21];
            string[] SangTDate_New = new string[21];
            int kk = 0;
            double nSangAmt_New = 0;
            int x = 0;
            int y = 0;
            string strToYear = ""; //'해당년도1월1일
            string strSangDate = ""; //'2015-01-13
            long nSangAmt = 0; //'2015-01-13
            string strGbSang = "";
            string strSugbs = "";
            long nAmt = 0;
            long nBoninAmt_Temp = 0;
            long n100BonAmt = 0;

            ComFunc CF = new ComFunc();

            nBonGubyo = GnTempAmt_Bon;

            if (ArgGubun == "1")
            {
                #region //ArgGubun = "1"
                nSangAmt = 0;

                //차상위2종 이면 입원일자가 2009-04-01 이후라야 상한제 적용됨
                if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && (clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "F" || clsPmpaType.TIT.OgPdBun == "J" || clsPmpaType.TIT.OgPdBun == "K"))
                {
                    if (string.Compare(clsPmpaType.TIT.InDate , "2009-04-01") < 0)
                    {
                        return 0;
                    }
                }
                //코로나검사 상한제 제외
                if (clsPmpaType.TIT.FCode == "MT04")
                {
                      return 0;
                  
                }

                //건강보험으로 최초입원일자 구함 - 상한제 처음날짜 2009-07-23 윤조연
                SQL = " SELECT TO_CHAR(b.INDATE,'YYYY-MM-DD') INDATE  FROM IPD_NEW_MASTER a, IPD_TRANS b  WHERE a.IPDNO=b.IPDNO   AND a.IPDNO = " + clsPmpaType.TIT.Ipdno + "   AND b.BI IN ('11','12','13')  " ; //  '11,12,13  건강보험만
                SQL = SQL + ComNum.VBLF + "   AND ( b.GBIPD NOT IN ('D') OR b.GBIPD IS NULL)  " ; //
                SQL = SQL + ComNum.VBLF + "  ORDER BY b.INDATE " ; //
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return 0;
                }
                if (dt.Rows.Count > 0)
                {
                    if (string.Compare(dt.Rows[0]["INDATE"].ToString().Trim(), "2004-06-30") <= 0)
                    {
                        SangFDate[0] = "2004-07-01";
                    }
                    else
                    {
                        SangFDate[0] = dt.Rows[0]["INDATE"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;

                for (i = 1; i <= 20; i++)
                {
                    if (i == 1)
                    {
                        SangFDate[i] = SangFDate[i-1];
                        SangTDate[i] = Limit_MagamDate(SangFDate[i]);
                    }
                    else
                    {
                        SangFDate[i] = CF.DATE_ADD(clsDB.DbCon, SangTDate[i - 1], 1);
                        SangTDate[i] = Limit_MagamDate(SangFDate[i]);
                    }
                }

                for (i = 1; i <= 20; i++)
                {
                    if (string.Compare(SangFDate[i] , clsPmpaType.TIT.InDate) <= 0 && string.Compare(SangTDate[i] , clsPmpaType.TIT.InDate) >= 0)
                    {
                        break;
                    }
                }

                if (i == 21) i = 20;

                SangFDate[i] = SangFDate[i];
                SangTDate[i] = SangTDate[i];

                //상한제 대상 쿼리
                SQL = " SELECT SUM(";
                for (J = 21; J <= 48; J++)
                {
                    SQL = SQL + ComNum.VBLF + " A.AMT" + J.ToString() + "+";
                }
                SQL = SQL + ComNum.VBLF + " A.AMT49) BIAMT, ";
                SQL = SQL + ComNum.VBLF + " SUM(AMT51+AMT57) AMT, SUM(AMT50) AMT50, SUM(AMT53) AMT53 ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.IPD_TRANS A ";
                SQL = SQL + ComNum.VBLF + " WHERE A.INDATE >= TO_DATE('" + SangFDate[i] + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "   AND A.OUTDATE <= TO_DATE('" + SangTDate[i] + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE IS NOT NULL " ;
                SQL = SQL + ComNum.VBLF + "   AND A.OUTDATE != TRUNC(SYSDATE) "; //당일 자신의 금액은 제외하고 계산해야함.
                //SQL = SQL + ComNum.VBLF + "   AND A.TRSNO <> " + clsPmpaType.TIT.Trsno + " ";
                SQL = SQL + ComNum.VBLF + "   AND A.GBIPD NOT IN ('D')  ";
                SQL = SQL + ComNum.VBLF + "   AND A.BI IN ('11','12','13') ";
                SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                SqlErr = clsDB.GetDataTableEx(ref dtSub, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return 0;
                }
                if (dtSub.Rows.Count > 0)
                {
                    nSangAmt_New = VB.Val(dtSub.Rows[0]["AMT50"].ToString().Trim()) - VB.Val(dtSub.Rows[0]["BIAMT"].ToString().Trim()) - VB.Val(dtSub.Rows[0]["AMT53"].ToString().Trim());
                }
                dtSub.Dispose();
                dtSub = null;

                clsPmpaType.TIT.SangAmt = 0;
                //상한제 2009년1월 부터는 400만원 본인부담금,나머지 금액은 공단 지원, 2009년 입원분만, 1년기간임.
                clsPmpaType.TIT.GbSang = "";

                if (string.Compare(clsPmpaType.TIT.Bi , "20") <= 0)
                {
                    if (string.Compare(clsPmpaType.TIT.InDate , "2009-01-01") >= 0)
                    {
                        for (i = 0; i <= 10; i++)
                        {
                            if (string.Compare(clsPmpaType.TIT.InDate , clsPmpaPb.GstrSangBdate[i]) >= 0)
                            {
                                strSangDate = clsPmpaPb.GstrSangBdate[i];
                                nSangAmt = clsPmpaPb.GnSangAmt[i];
                                break;
                            }
                        }

                        strGbSang = "8";
                        if (string.Compare(clsPmpaPb.GstrSangBdate[i] , "2018-01-01") >= 0)
                        {
                            strGbSang = "8";
                        }
                        else if (string.Compare(clsPmpaPb.GstrSangBdate[i], "2017-01-01") >= 0)
                        {
                            strGbSang = "7";
                        }
                        else if (string.Compare(clsPmpaPb.GstrSangBdate[i], "2016-01-01") >= 0)
                        {
                            strGbSang = "6";
                        }
                        else if (string.Compare(clsPmpaPb.GstrSangBdate[i], "2015-01-01") >= 0)
                        {
                            strGbSang = "5";
                        }
                        else if (string.Compare(clsPmpaPb.GstrSangBdate[i], "2014-01-01") >= 0)
                        {
                            strGbSang = "4";
                        }
                        else if (string.Compare(clsPmpaPb.GstrSangBdate[i], "2013-01-01") >= 0)
                        {
                            strGbSang = "3";
                        }
                        else if (string.Compare(clsPmpaPb.GstrSangBdate[i], "2012-01-01") >= 0)
                        {
                            strGbSang = "2";
                        }

                        if ((nBonGubyo + nSangAmt_New) > nSangAmt)
                        {
                            clsPmpaType.TIT.SangAmt = (long)nSangAmt_New + nBonGubyo - nSangAmt;

                            if (nSangAmt_New >= nSangAmt)
                            {
                                nBonGubyo = 0;
                                if (clsPmpaType.TIT.SangAmt > 0)
                                {
                                    clsPmpaType.TIT.GbSang = strGbSang;
                                }
                                else
                                {
                                    nBonGubyo = (nSangAmt - (long)nSangAmt_New);
                                    clsPmpaType.TIT.GbSang = strGbSang;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (string.Compare(clsPmpaType.TIT.InDate, "2007-07-01") >= 0 && nBonGubyo + nSangAmt_New > 2000000)
                        {
                            clsPmpaType.TIT.SangAmt = (long)nSangAmt_New + nBonGubyo - 2000000;
                            if (nSangAmt_New >= 2000000)
                            {
                                nBonGubyo = 0;
                            }
                            else
                            {
                                nBonGubyo = (2000000 - (long)nSangAmt_New);
                                clsPmpaType.TIT.GbSang = "2";
                            }
                        }
                        else if ((string.Compare(clsPmpaType.TIT.OutDate, "2004-07-01") >= 0 || clsPmpaType.TIT.OutDate == "") && nBonGubyo + nSangAmt_New > 3000000)
                        {
                            clsPmpaType.TIT.SangAmt = (long)nSangAmt_New + nBonGubyo - 3000000;
                            if (nSangAmt_New >= 3000000)
                            {
                                nBonGubyo = 0;
                            }
                            else
                            {
                                nBonGubyo = (3000000 - (long)nSangAmt_New);
                                clsPmpaType.TIT.GbSang = "1";
                            }
                        }
                    }
                }
                #endregion //ArgGubun = "1"
            }
            else
            {
                #region //else ArgGubun = "1"
                nSangAmt_New = 0;
                //차상위2종 이면 입원일자가 2009-04-01 이후라야 상한제 적용됨
                if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && (clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "F" || clsPmpaType.TIT.OgPdBun == "J" || clsPmpaType.TIT.OgPdBun == "K"))
                {
                    if (string.Compare(clsPmpaType.TIT.InDate , "2009-04-01") < 0)
                    {
                        return 0;
                    }
                }
                strToYear = VB.Left(clsPmpaType.TIT.InDate, 4) + "-01-01";

                //건강보험으로 최초입원일자 구함 - 상한제 처음날짜 2009-07-23 윤조연
                SQL = " SELECT TO_CHAR(b.INDATE,'YYYY-MM-DD') INDATE  FROM IPD_NEW_MASTER a, IPD_TRANS b   WHERE a.IPDNO=b.IPDNO ";
                SQL = SQL + ComNum.VBLF + "   AND a.Pano = '" + clsPmpaType.TIT.Pano + "'   AND b.InDate >= TO_DATE('" + strToYear + "','YYYY-MM-DD')   AND b.BI IN ('11','12','13')  " ;  //11,12,13  건강보험만
                SQL = SQL + ComNum.VBLF + "   AND ( b.GBIPD NOT IN ('D') OR b.GBIPD IS NULL)  " ;
                SQL = SQL + ComNum.VBLF + "  ORDER BY b.INDATE " ;
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return 0;
                }
                if (dt.Rows.Count > 0)
                {
                    if (string.Compare(dt.Rows[0]["INDATE"].ToString().Trim(), "2004-06-30") <= 0)
                    {
                        SangFDate_New[0] = "2004-07-01";
                    }
                    else
                    {
                        SangFDate_New[0] = dt.Rows[0]["INDATE"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;

                for (kk = 1; kk <= 20; kk++)
                {
                    if (kk == 1)
                    {
                        SangFDate_New[kk] = SangFDate_New[kk - 1];
                        SangTDate_New[kk] = Limit_MagamDate(SangFDate_New[kk]);
                    }
                    else
                    {
                        SangFDate_New[kk] = CF.DATE_ADD(clsDB.DbCon, SangTDate_New[kk - 1], 1);
                        SangTDate_New[kk] = Limit_MagamDate(SangFDate_New[kk]);
                    }
                }

                for (kk = 1; kk <= 20; kk++)
                {
                    if (string.Compare(SangFDate_New[kk], clsPmpaType.TIT.InDate) <= 0 && string.Compare(SangTDate_New[kk], clsPmpaType.TIT.InDate) >= 0)
                    {
                        break;
                    }
                }

                if (kk == 21) kk = 20;

                SangFDate_New[kk] = SangFDate_New[kk];
                SangTDate_New[kk] = SangTDate_New[kk];

                //선별급여중 본인부담금은 제외
                SQL = " SELECT TRSNO From IPD_TRANS A " ;
                SQL = SQL + ComNum.VBLF + " Where A.INDATE >= TO_DATE('" + SangFDate_New[kk] + "','YYYY-MM-DD') " ;
                SQL = SQL + ComNum.VBLF + "   AND A.OUTDATE <= TO_DATE('" + SangTDate_New[kk] + "','YYYY-MM-DD') " ;
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE IS NOT NULL " ;
                SQL = SQL + ComNum.VBLF + "   AND A.OUTDATE != TRUNC(SYSDATE) " ; //당일 자신의 금액은 제외하고 계산해야함.
                SQL = SQL + ComNum.VBLF + "   AND A.GBIPD NOT IN ('D') ";
                SQL = SQL + ComNum.VBLF + "   AND A.BI IN ('11','12','13') ";
                SQL = SQL + ComNum.VBLF + "   AND A.Pano = '" + clsPmpaType.TIT.Pano + "' " ;
                SqlErr = clsDB.GetDataTableEx(ref dtSub, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return 0;
                }
                if (dtSub.Rows.Count > 0)
                {
                    for (x = 0; x < dtSub.Rows.Count; x++)
                    {
                        SQL = "       SELECT SUM(Amt1) Amt,GbSuGbs ";
                        SQL = SQL + ComNum.VBLF + "   FROM IPD_NEW_SLIP ";
                        SQL = SQL + ComNum.VBLF + "  WHERE TRSNO=" + VB.Val(dtSub.Rows[x]["TRSNO"].ToString().Trim()) + " ";
                        SQL = SQL + ComNum.VBLF + "    AND GBSUGBS IN ('4','5','9') ";
                        SQL = SQL + ComNum.VBLF + "    AND GbSelf ='0' ";
                        SQL = SQL + ComNum.VBLF + "    AND BDate>=TO_DATE('2014-12-01','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  GROUP BY GbSuGbs ";
                        SQL = SQL + ComNum.VBLF + "  HAVING SUM(Amt1) != 0 ";
                        SqlErr = clsDB.GetDataTableEx(ref dtSub2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return 0;
                        }
                        if (dtSub2.Rows.Count > 0)
                        {
                            for (y = 0; y < dtSub2.Rows.Count; y++)
                            {
                                strSugbs = dtSub2.Rows[y]["GbSuGbs"].ToString().Trim();
                                nAmt = (long)VB.Val(dtSub2.Rows[y]["Amt"].ToString().Trim());

                                if (strSugbs == "4")
                                {
                                    n100BonAmt = n100BonAmt + (long)Math.Round((double)(nAmt * 80 / 100));
                                }
                                else if (strSugbs == "5")
                                {
                                    n100BonAmt = n100BonAmt + (long)Math.Round((double)(nAmt * 50 / 100));
                                }
                                else if (strSugbs == "9")
                                {
                                    n100BonAmt = n100BonAmt + (long)Math.Round((double)(nAmt * 90 / 100));
                                }
                            }
                        }
                        dtSub2.Dispose();
                        dtSub2 = null;

                        SQL = "       SELECT /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1) Amt,GbSuGbs ";
                        SQL = SQL + ComNum.VBLF + "   FROM IPD_NEW_SLIP ";
                        SQL = SQL + ComNum.VBLF + "  WHERE TRSNO=" + VB.Val(dtSub.Rows[x]["TRSNO"].ToString().Trim()) + " ";
                        SQL = SQL + ComNum.VBLF + "    AND GBSUGBS IN ('3','6','7','8') ";
                        SQL = SQL + ComNum.VBLF + "    AND GbSelf ='0' ";
                        SQL = SQL + ComNum.VBLF + "  GROUP BY GbSuGbs ";
                        SQL = SQL + ComNum.VBLF + "  HAVING SUM(Amt1) != 0 ";
                        SqlErr = clsDB.GetDataTableEx(ref dtSub2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return 0;
                        }
                        if (dtSub2.Rows.Count > 0)
                        {
                            for (y = 0; y < dtSub2.Rows.Count; y++)
                            {
                                strSugbs = dtSub2.Rows[y]["GbSuGbs"].ToString().Trim();
                                nAmt = (long)VB.Val(dtSub2.Rows[y]["Amt"].ToString().Trim());

                                if (strSugbs == "6")
                                {
                                    n100BonAmt = n100BonAmt + (long)Math.Round((double)(nAmt * 80 / 100)); //본인부담 80%
                                }
                                else if (strSugbs == "3")
                                {
                                    n100BonAmt = n100BonAmt + (long)Math.Round((double)(nAmt * 30 / 100));
                                }
                                else if (strSugbs == "7")
                                {
                                    n100BonAmt = n100BonAmt + (long)Math.Round((double)(nAmt * 50 / 100));
                                }
                                else if (strSugbs == "8")
                                {
                                    n100BonAmt = n100BonAmt + (long)Math.Round((double)(nAmt * 90 / 100));
                                }
                            }
                        }
                        dtSub2.Dispose();
                        dtSub2 = null;

                        //2인실 본인부담 제외 추가 2018-07-01
                        SQL = "       SELECT /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1) Amt ";
                        SQL = SQL + ComNum.VBLF + "   FROM IPD_NEW_SLIP ";
                        SQL = SQL + ComNum.VBLF + "  WHERE TRSNO=" + VB.Val(dtSub.Rows[x]["TRSNO"].ToString().Trim()) + " ";
                        SQL = SQL + ComNum.VBLF + "    AND sucode IN ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B','AV820A','AV820B','AV8201A','AV8201B') ";
                        SQL = SQL + ComNum.VBLF + "    AND GbSelf ='0' ";
                        SqlErr = clsDB.GetDataTableEx(ref dtSub2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return 0;
                        }
                        if (dtSub2.Rows.Count > 0)
                        {
                            for (y = 0; y < dtSub2.Rows.Count; y++)
                            {
                                nAmt = (long)VB.Val(dtSub2.Rows[y]["Amt"].ToString().Trim());
                                n100BonAmt = n100BonAmt + (long)Math.Round((double)(nAmt * 40 / 100));       //본인부담 40%
                            }
                        }
                        dtSub2.Dispose();
                        dtSub2 = null;


                        
                    }
                }
                dtSub.Dispose();
                dtSub = null;

                SQL = " SELECT SUM(";
                for (J = 21; J <= 48; J++)
                {
                    SQL = SQL + ComNum.VBLF + " A.AMT" + J.ToString() + "+";
                }
                SQL = SQL + ComNum.VBLF + " A.AMT49) BIAMT, ";
                if (string.Compare(clsPublic.GstrSysDate , "2011-04-01") >= 0)
                {
                    SQL = SQL + ComNum.VBLF + " SUM(AMT51+AMT57) AMT, SUM(AMT50-AMT64) AMT50, SUM(AMT53) AMT53 ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " SUM(AMT51+AMT57) AMT, SUM(AMT50) AMT50, SUM(AMT53) AMT53 ";
                }
                SQL = SQL + ComNum.VBLF + " FROM IPD_TRANS A  WHERE A.INDATE >= TO_DATE('" + SangFDate_New[kk] + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "   AND A.OUTDATE <= TO_DATE('" + SangTDate_New[kk] + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "   AND A.OUTDATE != TRUNC(SYSDATE) " ;
                SQL = SQL + ComNum.VBLF + "   AND A.GBIPD NOT IN ('D') ";
                SQL = SQL + ComNum.VBLF + "   AND NVL(A.FCODE,' ') <> 'MT04' ";
                SQL = SQL + ComNum.VBLF + "   AND A.BI IN ('11','12','13') " ;
                SQL = SQL + ComNum.VBLF + "   AND A.Pano = '" + clsPmpaType.TIT.Pano + "' " ;
                SqlErr = clsDB.GetDataTableEx(ref dtSub, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return 0;
                }
                if (dtSub.Rows.Count > 0)
                {
                    nSangAmt_New = VB.Val(dtSub.Rows[0]["AMT50"].ToString().Trim()) - VB.Val(dtSub.Rows[0]["BIAMT"].ToString().Trim()) - VB.Val(dtSub.Rows[0]["AMT53"].ToString().Trim()) - n100BonAmt;
                }
                dtSub.Dispose();
                dtSub = null;

                clsPmpaType.TIT.SangAmt = 0;
                clsPmpaType.TIT.GbSang = "";

                //상한금액계산시 장기입원료 본인부담추가 후 계산 2016-09-07
                nBoninAmt_Temp = nBonGubyo - GnTempHRoomBonin;

                if (string.Compare(clsPmpaType.TIT.Bi, "20") <= 0)
                {
                    if (string.Compare(clsPmpaType.TIT.InDate , "2009-01-01") >= 0)
                    {
                        for (i = 0; i <= 10; i++)
                        {
                            if (string.Compare(clsPmpaType.TIT.InDate , clsPmpaPb.GstrSangBdate[i])>= 0)
                            {
                                strSangDate = clsPmpaPb.GstrSangBdate[i];
                                nSangAmt = clsPmpaPb.GnSangAmt[i];
                                break;
                            }
                        }

                        strGbSang = "8";
                        if (string.Compare(clsPmpaPb.GstrSangBdate[i], "2018-01-01") >= 0)
                        {
                            strGbSang = "8";
                        }
                        else if (string.Compare(clsPmpaPb.GstrSangBdate[i], "2017-01-01") >= 0)
                        {
                            strGbSang = "7";
                        }
                        else if (string.Compare(clsPmpaPb.GstrSangBdate[i], "2016-01-01") >= 0)
                        {
                            strGbSang = "6";
                        }
                        else if (string.Compare(clsPmpaPb.GstrSangBdate[i], "2015-01-01") >= 0)
                        {
                            strGbSang = "5";
                        }
                        else if (string.Compare(clsPmpaPb.GstrSangBdate[i], "2014-01-01") >= 0)
                        {
                            strGbSang = "4";
                        }
                        else if (string.Compare(clsPmpaPb.GstrSangBdate[i], "2013-01-01") >= 0)
                        {
                            strGbSang = "3";
                        }
                        else if (string.Compare(clsPmpaPb.GstrSangBdate[i], "2012-01-01") >= 0)
                        {
                            strGbSang = "2";
                        }

                        if (nBoninAmt_Temp + nSangAmt_New > nSangAmt)
                        {
                            clsPmpaType.TIT.SangAmt = (long)nSangAmt_New + nBoninAmt_Temp - nSangAmt;
                            if (nSangAmt_New >= nSangAmt)
                            {
                                nBonGubyo = GnTempHRoomBonin;
                                if (clsPmpaType.TIT.SangAmt > 0)
                                {
                                    clsPmpaType.TIT.GbSang = strGbSang;
                                }
                            }
                            else
                            {
                                nBonGubyo = (nSangAmt - (long)nSangAmt_New) + GnTempHRoomBonin;
                                clsPmpaType.TIT.GbSang = strGbSang;
                            }
                        }
                    }
                    else
                    {
                        if (string.Compare(clsPmpaType.TIT.InDate, "2007-07-01") >= 0 && nBonGubyo + nSangAmt_New > 2000000)
                        {
                            clsPmpaType.TIT.SangAmt = (long)nSangAmt_New + nBonGubyo - 2000000;
                            if (nSangAmt_New >= 2000000)
                            {
                                nBonGubyo = 0;
                            }
                            else
                            {
                                nBonGubyo = (2000000 - (long)nSangAmt_New);
                                clsPmpaType.TIT.GbSang = "2";
                            }
                        }
                        else if ((string.Compare(clsPmpaType.TIT.OutDate , "2004-07-01") >= 0 || clsPmpaType.TIT.OutDate == "") && nBonGubyo + nSangAmt_New > 3000000)
                        {
                            clsPmpaType.TIT.SangAmt = (long)nSangAmt_New + nBonGubyo - 3000000;
                            if (nSangAmt_New >= 3000000)
                            {
                                nBonGubyo = 0;
                            }
                            else
                            {
                                nBonGubyo = (3000000 - (long)nSangAmt_New);
                                clsPmpaType.TIT.GbSang = "1";
                            }
                        }
                    }
                }
                #endregion //else ArgGubun = "1"
            }

            GnTempAmt_Bon = 0;
            GnTempHRoomBonin = 0;
            return nBonGubyo;
        }

        /// <summary>
        /// 상한제_마감일자
        /// Author : 박웅규
        /// </summary>
        /// <param name="argDATE"></param>
        /// <returns></returns>
        /// <seealso cref="VbFunction.bas : 상한제_마감일자"/>
        private string Limit_MagamDate(string argDATE)
        {

            string strStartDate        = "";
            string strEndDate          = "";
            string strDD               = "";
            string strMM               = "";
            string strEndMM            = "";
            string strEndYear = "";

            ComFunc CF = new ComFunc();

            if (string.Compare(argDATE , "2004-06-30") <= 0)
            {
                argDATE = "2004-07-01";
            }

            if (string.Compare(argDATE, "2009-01-01") >= 0)
            {
                strStartDate = VB.Left(argDATE, 10);
                strMM = VB.Mid(strStartDate, 6, 2);
                strDD = VB.Right(strStartDate, 2);
                strEndYear = VB.Left(strStartDate, 4);
                strEndDate = strEndYear + "-12-31";
            }
            else if (string.Compare(argDATE, "2008-12-31") <= 0)
            {
                strStartDate = VB.Left(argDATE, 10);
                strMM = VB.Mid(strStartDate, 6, 2);
                strDD = VB.Right(strStartDate, 2);
                strEndYear = VB.Left(strStartDate, 4);
                strEndMM = (VB.Val(strMM) + 6).ToString("00"); 

                if (VB.Val(strEndMM) >= 13)
                {
                    strEndMM = (VB.Val(strEndMM) - 12).ToString("00");
                    strEndYear = (VB.Val(strEndYear) + 1).ToString();
                }

                if (strDD == "01")
                {
                    strEndDate = CF.DATE_ADD(clsDB.DbCon, strEndYear + "-" + strEndMM + "-" + strDD, -1);
                }
                else
                {
                    strEndDate = strEndYear + "-" + strEndMM + "-" + strDD;
                    if ((string.Compare((strEndMM + strDD) , "0229") >= 0 && string.Compare((strEndMM + strDD) , "0231") <= 0) || (strEndMM + strDD) == "0431" || (strEndMM + strDD) == "0631" || (strEndMM + strDD) == "0931" || (strEndMM + strDD) == "1131" )
                    {
                        strEndDate = CF.READ_LASTDAY(clsDB.DbCon, strEndYear + "-" + strEndMM + "-01");
                    }
                    else
                    {
                        strEndDate = CF.DATE_ADD(clsDB.DbCon, strEndDate, -1);
                    }
                }
            }
            CF = null;

            return strEndDate;
        }

        /// <summary>
        /// Description : DRG 세부내역 표시
        /// Author : 김민철
        /// Create Date : 2018.02.01
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPano"></param>
        /// <param name="IpdNo"></param>
        /// <param name="TrsNo"></param>
        /// <returns></returns>
        public void Ipd_Tewon_PrtAmt_Gesan_Drg(PsmhDb pDbCon, FpSpread ssDrg, FpSpread ssDrgAmt, string strPano, long IpdNo, long TrsNo)
        {
            int nRow = 0;
            long nTotAmt = 0;
            long nBonAmt = 0;
            long nJhpAmt = 0;

            if (clsPmpaType.TIT.Amt[70] > 0 && 1==2 ) // 구현오류 임시막음
            {
                #region 고도화 작업
                //DRG 원금액
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = clsPmpaType.TIT.Amt[71].ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (clsPmpaType.TIT.Amt[71] - clsPmpaType.TIT.Amt[92]).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = clsPmpaType.TIT.Amt[92].ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 추가입원료   81
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = clsPmpaType.TIT.Amt[81].ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = clsPmpaType.TIT.Amt[93].ToString("###,###,##0");   //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = clsPmpaType.TIT.Amt[94].ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 의료질평가지원금
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = clsPmpaType.TIT.Amt[76].ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (clsPmpaType.TIT.Amt[76] - (long)Math.Truncate(clsPmpaType.TIT.Amt[76] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = ((long)Math.Truncate(clsPmpaType.TIT.Amt[76] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 안전관리료
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = clsPmpaType.TIT.Amt[90].ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (clsPmpaType.TIT.Amt[90] - (long)Math.Truncate(clsPmpaType.TIT.Amt[90] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = ((long)Math.Truncate(clsPmpaType.TIT.Amt[90] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 외과가산금액
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = clsPmpaType.TIT.Amt[77].ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (clsPmpaType.TIT.Amt[77] - (long)Math.Truncate(clsPmpaType.TIT.Amt[77] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = ((long)Math.Truncate(clsPmpaType.TIT.Amt[77] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 부수술총액  82
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = clsPmpaType.TIT.Amt[82].ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (clsPmpaType.TIT.Amt[82] - (long)Math.Truncate(clsPmpaType.TIT.Amt[82] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = ((long)Math.Truncate(clsPmpaType.TIT.Amt[82] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 복강개복   79
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = clsPmpaType.TIT.Amt[79].ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (clsPmpaType.TIT.Amt[79] - (long)Math.Truncate(clsPmpaType.TIT.Amt[79] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = ((long)Math.Truncate(clsPmpaType.TIT.Amt[79] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";
                
                //DRG 응급가산수가  84
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = clsPmpaType.TIT.Amt[84].ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (clsPmpaType.TIT.Amt[84] - (long)Math.Truncate(clsPmpaType.TIT.Amt[84] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = ((long)Math.Truncate(clsPmpaType.TIT.Amt[84] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 재왕절개수가  86
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = clsPmpaType.TIT.Amt[86].ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (clsPmpaType.TIT.Amt[86] - (long)Math.Truncate(clsPmpaType.TIT.Amt[86] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = ((long)Math.Truncate(clsPmpaType.TIT.Amt[86] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 급여초음파  85
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = clsPmpaType.TIT.Amt[85].ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (clsPmpaType.TIT.Amt[85] - (long)Math.Truncate(clsPmpaType.TIT.Amt[85] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = ((long)Math.Truncate(clsPmpaType.TIT.Amt[85] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 간호간병료  83
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = clsPmpaType.TIT.Amt[83].ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (clsPmpaType.TIT.Amt[83] - (long)Math.Truncate(clsPmpaType.TIT.Amt[83] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = ((long)Math.Truncate(clsPmpaType.TIT.Amt[83] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 선별급여  87, 88, 89
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = (clsPmpaType.TIT.Amt[87] + clsPmpaType.TIT.Amt[88] + clsPmpaType.TIT.Amt[89]).ToString("###,###,##0");
                //ssDrg.ActiveSheet.Cells[nRow, 2].Text = ((clsPmpaType.TIT.Amt[87] + clsPmpaType.TIT.Amt[88] + clsPmpaType.TIT.Amt[89]) - (long)Math.Truncate((clsPmpaType.TIT.Amt[87] + clsPmpaType.TIT.Amt[88] + clsPmpaType.TIT.Amt[89]) * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //공단
                //ssDrg.ActiveSheet.Cells[nRow, 3].Text = ((long)Math.Truncate((clsPmpaType.TIT.Amt[87] + clsPmpaType.TIT.Amt[88] + clsPmpaType.TIT.Amt[89]) * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = ((clsPmpaType.TIT.Amt[87] + clsPmpaType.TIT.Amt[88] + clsPmpaType.TIT.Amt[89]) - ((long)Math.Truncate((clsPmpaType.TIT.Amt[87]) * 0.8) + (long)Math.Truncate((clsPmpaType.TIT.Amt[88]) * 0.5) + (long)Math.Truncate((clsPmpaType.TIT.Amt[89]) * 0.9))).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = ((long)Math.Truncate((clsPmpaType.TIT.Amt[87]) * 0.8)+ (long)Math.Truncate((clsPmpaType.TIT.Amt[88]) * 0.5)+ (long)Math.Truncate((clsPmpaType.TIT.Amt[89]) * 0.9)).ToString("###,###,##0");    //본인

                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 식대본인부담 계산
                clsPmpaType.RPG.Amt5[3] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[3] * clsPmpaType.IBR.Food / 100.0);
                clsPmpaType.RPG.Amt6[3] = clsPmpaType.RPG.Amt1[3] - clsPmpaType.RPG.Amt5[3];
                clsPmpaType.RPG.Amt5[50] += clsPmpaType.RPG.Amt5[3];
                clsPmpaType.RPG.Amt6[50] += clsPmpaType.RPG.Amt6[3];

                //식대계산
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = clsPmpaType.RPG.Amt1[3].ToString("###,###,##0");  //총액
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = clsPmpaType.RPG.Amt6[3].ToString("###,###,##0");  //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[3].ToString("###,###,##0");  //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = clsPmpaType.RPG.Amt4[3].ToString("###,###,##0");  //전액
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt3[3].ToString("###,###,##0");  //선택
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt2[3].ToString("###,###,##0");  //비급여

                //DRG 인정비급여 78
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = clsPmpaType.TIT.Amt[78].ToString("###,###,##0");
              //  ssDrg.ActiveSheet.Cells[nRow, 2].Text = (clsPmpaType.TIT.Amt[78] - (long)Math.Truncate(clsPmpaType.TIT.Amt[78] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = "0";    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = "0";    //본인
              //   ssDrg.ActiveSheet.Cells[nRow, 4].Text = ((long)Math.Truncate(clsPmpaType.TIT.Amt[78] * clsPmpaType.IBR.Bohum / 100.0)).ToString("###,###,##0"); ;
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = clsPmpaType.TIT.Amt[78].ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";
                #endregion
            }
            else
            {
                #region 고도화 이전
                //DRG 원금액
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = DRG.GnDRG_Amt1.ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (DRG.GnDRG_Amt1 - DRG.GnDRG_WBonAmt).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = DRG.GnDRG_WBonAmt.ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 추가입원료   81
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = DRG.GnDrg추가입원료.ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (DRG.GnDrg추가입원료 - DRG.GnDrg추가입원료_Bon).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = DRG.GnDrg추가입원료_Bon.ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 의료질평가지원금
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = DRG.GnDrgJinAmt.ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (DRG.GnDrgJinAmt - DRG.GnDrgJinAmt_Bon).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = DRG.GnDrgJinAmt_Bon.ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 안전관리료
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = DRG.GnDrgJinSAmt.ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (DRG.GnDrgJinSAmt - DRG.GnDrgJinSAmt_Bon).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = DRG.GnDrgJinSAmt_Bon.ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 외과가산금액
                nRow += 1;
                nBonAmt = (long)Math.Truncate(DRG.GnGsAddAmt * clsPmpaType.IBR.Bohum / 100.0);
                nJhpAmt = DRG.GnGsAddAmt - nBonAmt;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = DRG.GnGsAddAmt.ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = nJhpAmt.ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = nBonAmt.ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 부수술총액  82
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = DRG.GnDrg부수술총액.ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (DRG.GnDrg부수술총액 - DRG.GnDrg부수술총액_Bon).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = DRG.GnDrg부수술총액_Bon.ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 복강개복   79
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = DRG.Gn복강개복Amt.ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = "0";    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = "0";    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";
                
                //DRG 응급가산수가  84
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = DRG.Gn응급가산수가.ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (DRG.Gn응급가산수가 - DRG.Gn응급가산수가_Bon).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = DRG.Gn응급가산수가_Bon.ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 재왕절개수가  86
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = (DRG.Gn재왕절개수가+ DRG.GnPCA).ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (DRG.Gn재왕절개수가 - DRG.Gn재왕절개수가_Bon + DRG.GnPCA - DRG.GnPCA_Bon ).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = (DRG.Gn재왕절개수가_Bon + DRG.GnPCA_Bon).ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 급여초음파  85
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = DRG.GnDrgSono.ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (DRG.GnDrgSono - DRG.GnDrgSono_Bon).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = DRG.GnDrgSono_Bon.ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 간호간병료  83
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = DRG.GnDrg간호간병료.ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = (DRG.GnDrg간호간병료 - DRG.GnDrg간호간병료_Bon).ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = DRG.GnDrg간호간병료_Bon.ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 선별급여  87, 88, 89
                nRow += 1;
                nTotAmt = DRG.GnGs80Amt_T + DRG.GnGs50Amt_T + DRG.GnGs90Amt_T;
                nBonAmt = DRG.GnGs80Amt_B + DRG.GnGs50Amt_B + DRG.GnGs90Amt_B;
                nJhpAmt = DRG.GnGs80Amt_J + DRG.GnGs50Amt_J + DRG.GnGs90Amt_J;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = nTotAmt.ToString("###,###,##0");
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = nJhpAmt.ToString("###,###,##0");    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = nBonAmt.ToString("###,###,##0");    //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG 식대본인부담 계산

                //clsPmpaType.RPG.Amt5[3] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[3] * clsPmpaType.IBR.Food / 100.0);
                //clsPmpaType.RPG.Amt6[3] = clsPmpaType.RPG.Amt1[3] - clsPmpaType.RPG.Amt5[3];// + DRG.GnDrgFoodAmt;
                clsPmpaType.RPG.Amt5[3] = DRG.GnDrgFoodAmt[0];
                clsPmpaType.RPG.Amt6[3] = DRG.GnDrgFoodAmt[1];
                clsPmpaType.RPG.Amt5[50] += clsPmpaType.RPG.Amt5[3];
                clsPmpaType.RPG.Amt6[50] += clsPmpaType.RPG.Amt6[3];

                //식대계산
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = clsPmpaType.RPG.Amt1[3].ToString("###,###,##0");  //총액
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = clsPmpaType.RPG.Amt6[3].ToString("###,###,##0");  //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[3].ToString("###,###,##0");  //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = clsPmpaType.RPG.Amt4[3].ToString("###,###,##0");  //전액
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt2[3].ToString("###,###,##0");  //비급여
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt3[3].ToString("###,###,##0");  //선택

                //DRG 인정비급여 78
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = "0";    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = "0";   //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = DRG.GnGs100Amt.ToString("###,###,##0"); 
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                //DRG GnOTChaAmt            
                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = DRG.GnOTChaAmt.ToString("###,###,##0"); ;
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = DRG.GnOTChaAmt_Jhp.ToString("###,###,##0"); ;    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = DRG.GnOTChaAmt_Bon.ToString("###,###,##0"); ;   //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";   //본인
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = DRG.GnDrgADDAmt.ToString("###,###,##0"); ;
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = DRG.GnDrgADDAmt_Jhp.ToString("###,###,##0"); ;    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = DRG.GnDrgADDAmt_Bon.ToString("###,###,##0"); ;   //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";   //본인
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";

                nRow += 1;
                ssDrg.ActiveSheet.Cells[nRow, 1].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 2].Text = "0";    //공단
                ssDrg.ActiveSheet.Cells[nRow, 3].Text = "0";   //본인
                ssDrg.ActiveSheet.Cells[nRow, 4].Text = "0";   //본인
                ssDrg.ActiveSheet.Cells[nRow, 5].Text = "0";
                ssDrg.ActiveSheet.Cells[nRow, 6].Text = "0";
                #endregion
            }

            //DRG 총내역 표시 2018-08-20 KMC
            //DRG 급여총액
            nTotAmt = (DRG.GnDRG_TAmt - DRG.GnDrg열외군금액_Bon - DRG.GnGs100Amt - DRG.GnDrgBiTAmt);
            // nBonAmt = nTotAmt - (DRG.GnDRG_TAmt - clsPmpaType.TIT.Amt[55]);
            nBonAmt = nTotAmt - (DRG.GnDRG_TAmt - (DRG.GnDrgBonAmt + DRG.GnDrgBiTAmt + DRG.GnDrgFoodAmt[0] + DRG.GnDrgRoomAmt[0] + DRG.GnGs100Amt + DRG.GnGs80Amt_B+ DRG.GnGs50Amt_B+ DRG.GnGs90Amt_B));
            nJhpAmt = nTotAmt - nBonAmt;
            ssDrg.ActiveSheet.Cells[17, 1].Text = nTotAmt.ToString("###,###,##0");                      //DRG 급여총액
            ssDrg.ActiveSheet.Cells[17, 2].Text = nJhpAmt.ToString("###,###,##0");                      //DRG 급여총액(공단)
            ssDrg.ActiveSheet.Cells[17, 3].Text = nBonAmt.ToString("###,###,##0");                      //DRG 급여총액(본인)
            ssDrg.ActiveSheet.Cells[17, 4].Text = clsPmpaType.TIT.Amt[78].ToString("###,###,##0");      //DRG 인정비급여
            ssDrg.ActiveSheet.Cells[17, 5].Text = clsPmpaType.TIT.Amt[73].ToString("###,###,##0");      //DRG 비급여
            ssDrg.ActiveSheet.Cells[17, 6].Text = clsPmpaType.TIT.Amt[75].ToString("###,###,##0");      //DRG 선택진료
            ssDrg.ActiveSheet.Cells[19, 1].Text = clsPmpaType.TIT.Amt[55].ToString("###,###,##0");      //본인부담금
            ssDrg.ActiveSheet.Cells[20, 1].Text = DRG.Gn행위별총액.ToString("###,###,##0");             //행위별총액

            if (clsPmpaType.TIT.Amt[70] > 0 && 1 == 2)
            {
                ssDrg.ActiveSheet.Cells[17, 1].Text = clsPmpaType.TIT.Amt[72].ToString("###,###,##0");                      //DRG 급여총액
                ssDrg.ActiveSheet.Cells[17, 2].Text = clsPmpaType.TIT.Amt[53].ToString("###,###,##0");                     //DRG 급여총액(공단)
                ssDrg.ActiveSheet.Cells[17, 3].Text = clsPmpaType.TIT.Amt[70].ToString("###,###,##0");                      //DRG 급여총액(본인)
                
                ssDrg.ActiveSheet.Cells[20, 1].Text = clsPmpaType.TIT.Amt[95].ToString("###,###,##0");      //행위별총액


                ssDrg.ActiveSheet.Cells[18, 1].Text = clsPmpaType.TIT.Amt[70].ToString("###,###,##0");    //DRG 총액
                ssDrg.ActiveSheet.Cells[21, 1].Text = clsPmpaType.TIT.Amt[80].ToString("###,###,##0");    //행위별총액

                ssDrgAmt.ActiveSheet.Cells[0, 1].Text = clsPmpaType.TIT.Amt[70].ToString("###,###,##0");  //DRG 총액
                ssDrgAmt.ActiveSheet.Cells[1, 1].Text = clsPmpaType.TIT.Amt[72].ToString("###,###,##0");  //DRG 급여총액
                ssDrgAmt.ActiveSheet.Cells[2, 1].Text = clsPmpaType.TIT.Amt[73].ToString("###,###,##0");  //DRG 비급여총액
                ssDrgAmt.ActiveSheet.Cells[3, 1].Text = clsPmpaType.TIT.Amt[80].ToString("###,###,##0");  //DRG 열외군금액
                ssDrgAmt.ActiveSheet.Cells[4, 1].Text = clsPmpaType.TIT.Amt[71].ToString("###,###,##0");  //DRG 원금액
                ssDrgAmt.ActiveSheet.Cells[5, 1].Text = clsPmpaType.TIT.Amt[75].ToString("###,###,##0");  //DRG 선택진료합계
                ssDrgAmt.ActiveSheet.Cells[6, 1].Text = clsPmpaType.TIT.DRGOG == "" ? "N" : "Y";          //DRG 산과가산여부
            }
            else
            {
                ssDrg.ActiveSheet.Cells[18, 1].Text = (DRG.GnDRG_TAmt - DRG.GnDrg열외군금액_Bon).ToString("###,###,##0");    //DRG 총액
                ssDrg.ActiveSheet.Cells[21, 1].Text = DRG.GnDrg열외군금액.ToString("###,###,##0");    //행위별총액

                ssDrgAmt.ActiveSheet.Cells[0, 1].Text = (DRG.GnDRG_TAmt - DRG.GnDrg열외군금액_Bon).ToString("###,###,##0");    //DRG 총액
                ssDrgAmt.ActiveSheet.Cells[1, 1].Text = (DRG.GnDRG_TAmt - DRG.GnDrg열외군금액_Bon - DRG.GnGs100Amt - DRG.GnDrgBiTAmt).ToString("###,###,##0");    //DRG 급여총액
                ssDrgAmt.ActiveSheet.Cells[2, 1].Text = DRG.GnDrgBiFAmt.ToString("###,###,##0");    //DRG 비급여총액
                ssDrgAmt.ActiveSheet.Cells[3, 1].Text = DRG.GnDrg열외군금액.ToString("###,###,##0");    //DRG 열외군금액
                ssDrgAmt.ActiveSheet.Cells[4, 1].Text = DRG.GnDRG_Amt1.ToString("###,###,##0");    //DRG 원금액
                ssDrgAmt.ActiveSheet.Cells[5, 1].Text = DRG.GnDrgSelTAmt.ToString("###,###,##0");    //DRG 선택진료합계
                ssDrgAmt.ActiveSheet.Cells[6, 1].Text = DRG.GstrOgAdd == "1" ? "Y" : "N";            //DRG 산과가산여부
            }

            ssDrgAmt.ActiveSheet.Cells[7, 1].Text = clsPmpaType.TIT.Amt[53].ToString("###,###,##0");    //공단부담금
            ssDrgAmt.ActiveSheet.Cells[8, 1].Text = clsPmpaType.TIT.Amt[55].ToString("###,###,##0");    //본인부담금
            ssDrgAmt.ActiveSheet.Cells[9, 1].Text = clsPmpaType.TIT.Amt[51].ToString("###,###,##0");    //이미납부액
            ssDrgAmt.ActiveSheet.Cells[10, 1].Text = clsPmpaType.TIT.Amt[54].ToString("###,###,##0");    //할인금액
            ssDrgAmt.ActiveSheet.Cells[11, 1].Text = clsPmpaType.TIT.Amt[52].ToString("###,###,##0");    //지원금
            ssDrgAmt.ActiveSheet.Cells[12, 1].Text = clsPmpaType.TIT.Amt[57].ToString("###,###,##0");    //수납금액
            ssDrgAmt.ActiveSheet.Cells[13, 1].Text = DRG.GnDrg열외군선별.ToString("###,###,##0");        //열외군선별행위기준
           
        }

        /// <summary>
        /// Description : RPG.AMT 변수에 본인부담금 세팅
        /// Author : 김민철
        /// Create Date : 2018.02.01
        /// </summary>
        /// <param name="pDbCon"></param>
        public void Ipd_RPG_Amt_Set(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0;

            long nAmt = 0, nAMT09 = 0, nAMT85 = 0, nAMT09_H = 0, nAMT85_H = 0;
            long nKekliAmt = 0, nKekliAmt_Bon = 0;
            long nHRoomAmt = 0, nHRoomBonin = 0;
            long nAmtDrug11 = 0, nAmtDrug20 = 0;  //퇴장방지 약가

            try
            {

                #region 입원료 본인부담 선별대상 구분
                if (string.Compare(clsPmpaType.TIT.M_InDate, clsPmpaPb.GstrLngRtDate) >= 0 && clsPmpaType.TIT.FCode != "F014" && VB.Left(clsPmpaType.TIT.Bi, 1) == "1" && clsPmpaType.TIT.VCode.Trim() == "" && clsPmpaType.TIT.OgPdBun.Trim() == "")
                {

                    SQL = "";
                    SQL += ComNum.VBLF + "  SELECT a.QTY,SUM(a.AMT1) Amt ";
                    SQL += ComNum.VBLF + "    From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                    SQL += ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_SUN b ";
                    SQL += ComNum.VBLF + "   Where a.TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                    SQL += ComNum.VBLF + "     AND a.SUNEXT = b.SUNEXT(+)  ";
                    SQL += ComNum.VBLF + "     AND b.DTLBUN = '1100' ";
                    SQL += ComNum.VBLF + "     AND a.QTY <= 0.9 ";
                    SQL += ComNum.VBLF + "     AND a.QTY >= 0.85  ";
                    SQL += ComNum.VBLF + "     AND a.SUNEXT not in ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B','AV820A','AV820B','AV8201A','AV8201B') ";
                    SQL += ComNum.VBLF + "   GROUP By a.QTY ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        for (i = 0; i < Dt.Rows.Count; i++)
                        {
                            if (Dt.Rows[i]["Amt"].ToString() != "")
                            {
                                nAmt = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());
                            }

                            if (Convert.ToDouble(Dt.Rows[i]["QTY"].ToString()) == 0.9)
                            {
                                nAMT09 = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString()); //본인부담 25%
                            }
                            else
                            {
                                nAMT85 = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString()); //본인부담 30%
                            }

                            clsPmpaType.RPG.Amt1[2] -= nAmt;
                        }
                    }
                    Dt.Dispose();
                    Dt = null;

                }
                #endregion

                nAmt = 0;
                #region 입원료 본인부담 선별대상 구분
                if (string.Compare(clsPmpaType.TIT.M_InDate, "2020-01-01") >= 0 && clsPmpaType.TIT.FCode != "F014" && VB.Left(clsPmpaType.TIT.Bi, 1) == "1" && clsPmpaType.TIT.VCode.Trim() == "" && clsPmpaType.TIT.OgPdBun.Trim() == "")
                {

                    SQL = "";
                    SQL += ComNum.VBLF + "  SELECT a.QTY,SUM(a.AMT1) Amt ";
                    SQL += ComNum.VBLF + "    From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                    SQL += ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_SUN b ";
                    SQL += ComNum.VBLF + "   Where a.TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                    SQL += ComNum.VBLF + "     AND a.SUNEXT = b.SUNEXT(+)  ";
                    SQL += ComNum.VBLF + "     AND b.DTLBUN = '1100' ";
                    SQL += ComNum.VBLF + "     AND a.QTY <= 0.9 ";
                    SQL += ComNum.VBLF + "     AND a.QTY >= 0.85  ";
                    SQL += ComNum.VBLF + "     AND a.SUNEXT  in ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B') ";
                    SQL += ComNum.VBLF + "   GROUP By a.QTY ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        for (i = 0; i < Dt.Rows.Count; i++)
                        {
                            if (Dt.Rows[i]["Amt"].ToString() != "")
                            {
                                nAmt = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());
                            }

                            if (Convert.ToDouble(Dt.Rows[i]["QTY"].ToString()) == 0.9)
                            {
                                nAMT09_H = (long)Math.Truncate(nAmt * 5 / 100.0);   //본인부담 5%가산   
                            }
                            else
                            {
                                nAMT85_H = (long)Math.Truncate(nAmt * 10 / 100.0);   //본인부담 10%가산   
                            }

                        
                        }
                    }
                    Dt.Dispose();
                    Dt = null;

                }
                #endregion
                nAmt = 0;


                #region 격리병실료 산정 
                if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0 && clsPmpaType.IBR.Bohum > 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1) KekliAmt ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE PANO ='" + clsPmpaType.TIT.Pano + "' ";
                    SQL += ComNum.VBLF + "    AND TRSNO= " + clsPmpaType.TIT.Trsno + " ";
                    SQL += ComNum.VBLF + "    AND SUCode IN ('AJ010','AJ020','AK200','AK201','AK202','AK210','AK211','V6001','V6002','AK200A','AH001','AH002') ";
                    SQL += ComNum.VBLF + "    AND BDate>=TO_DATE('2016-09-23','YYYY-MM-DD') ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        for (i = 0; i < Dt.Rows.Count; i++)
                        {
                            if (Dt.Rows[i]["KekliAmt"].ToString() != "")
                            {
                                nAmt = Convert.ToInt64(Dt.Rows[i]["KekliAmt"].ToString());
                            }

                            nKekliAmt += nAmt;
                            if (clsPmpaType.TIT.VCode != "")
                            {
                                if (VB.Left(clsPmpaType.TIT.Bi, 1) == "1")
                                {
                                    if (clsPmpaType.TIT.VCode == "V000" || clsPmpaType.TIT.VCode == "V010")
                                    {
                                        nKekliAmt_Bon += (long)Math.Truncate(nAmt * (0 / 100.0));
                                    }
                                    else
                                    {
                                        nKekliAmt_Bon += (long)Math.Truncate(nAmt * (clsPmpaType.IBR.Bohum / 100.0));
                                    }
                                }
                            }
                            else
                            {
                                if (VB.Left(clsPmpaType.TIT.Bi, 1) == "1")
                                {
                                    if (clsPmpaType.TIT.OgPdBun == "E"  && clsPmpaType.TIT.Age < 6)
                                    {
                                        //nKekliAmt_Bon += (long)Math.Truncate(nAmt * (0 / 100.0));
                                    }
                                    else if(clsPmpaType.TIT.OgPdBun == "E"  && clsPmpaType.TIT.Age <= 15 )
                                    {
                                        nKekliAmt_Bon += (long)Math.Truncate(nAmt * (3 / 100.0));
                                    }
                                    else if (clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2" || clsPmpaType.TIT.OgPdBun == "V" || clsPmpaType.TIT.OgPdBun == "H" || clsPmpaType.TIT.OgPdBun == "S" || clsPmpaType.TIT.OgPdBun == "Y")
                                    {
                                        nKekliAmt_Bon += (long)Math.Truncate(nAmt * (5 / 100.0));
                                    }
                                    else if (clsPmpaType.TIT.OgPdBun == "F"|| clsPmpaType.TIT.OgPdBun == "C" || clsPmpaType.TIT.OgPdBun == "P")
                                    {
                                        nKekliAmt_Bon += (long)Math.Truncate(nAmt * (0 / 100.0));
                                    }
                                    else
                                    {
                                        nKekliAmt_Bon += (long)Math.Truncate(nAmt * (10 / 100.0));
                                    }
                                }
                                else if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.OgPdBun == "P")
                                {
                                   // nKekliAmt_Bon += (long)Math.Truncate(nAmt * (0 / 100.0));
                                }
                                else if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.OgPdBun == "Y")
                                {
                                    nKekliAmt_Bon += (long)Math.Truncate(nAmt * (3 / 100.0));
                                }
                                else if (clsPmpaType.TIT.Bi == "22")
                                {
                                    //2021-06-17 의뢰서
                                    nKekliAmt_Bon += (long)Math.Truncate(nAmt * (10 / 100.0));
                                    //nKekliAmt_Bon += (long)Math.Truncate(nAmt * (5 / 100.0));
                                }
                            }
                             clsPmpaType.RPG.Amt1[2] -= nAmt; 
                        }
                    }
                    Dt.Dispose();
                    Dt = null;

                }
                #endregion
                nAmt = 0;
                #region 2인실 병실료산정
                if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0 )
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1) HRoomAmt  ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE PANO ='" + clsPmpaType.TIT.Pano + "' ";
                    SQL += ComNum.VBLF + "    AND TRSNO= " + clsPmpaType.TIT.Trsno + " ";
                    SQL += ComNum.VBLF + "    AND SUCode IN ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B','AV820A','AV820B','AV8201A','AV8201B') ";
      
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        for (i = 0; i < Dt.Rows.Count; i++)
                        {
                            if (Dt.Rows[i]["HRoomAmt"].ToString() != "")
                            {
                                nAmt = Convert.ToInt64(Dt.Rows[i]["HRoomAmt"].ToString());
                            }

                            nHRoomAmt += nAmt;
                            nHRoomBonin += (long)Math.Truncate(nAmt * (40 / 100.0));
                            
                            clsPmpaType.RPG.Amt1[2] -= nAmt;
                        }
                    }
                    nHRoomBonin += nAMT09_H + nAMT85_H;
                    Dt.Dispose();
                    Dt = null;

                }
                #endregion
               
                #region 퇴장방지 약제 가산단가 본인부담 제외 
                if ((string.Compare(clsPmpaType.TIT.Bi, "30") < 0) && (clsPmpaType.TIT.DrgCode.Trim() == ""))
                {
                  
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT a.bun,round(SUM(a.AMT1) -  (SUM(a.AMT1) / 1.1) )   Amt ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a ," + ComNum.DB_PMPA + "BAS_SUN b";
                    SQL += ComNum.VBLF + "  WHERE a.TRSNO= " + clsPmpaType.TIT.Trsno + " ";
                    SQL += ComNum.VBLF + "    AND a.SUNEXT = b.SUNEXT(+)";
                    SQL += ComNum.VBLF + "    AND a.GbSelf ='0' ";
                    SQL += ComNum.VBLF + "    AND b.sugbm = '1' ";
                    SQL += ComNum.VBLF + "    AND a.BDate >= TO_DATE('2020-03-01','YYYY-MM-DD')         ";
                    SQL += ComNum.VBLF + "    group by a.bun  ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (Dt.Rows.Count > 0) //long nAmtDrug11 = 0, nAmtDrug20 = 0;  //퇴장방지 약가
                    {
                        for (i = 0; i < Dt.Rows.Count; i++)
                        {
                            if (Dt.Rows[i]["bun"].ToString() == "11")
                            {
                                nAmtDrug11 = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());
                            }
                            else if (Dt.Rows[i]["bun"].ToString() == "20")
                            {
                                nAmtDrug20 = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());
                            }


                        }
                    }

                    Dt.Dispose();
                    Dt = null;

                }
                #endregion



                clsPmpaType.RPG.Amt5[1] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[1] * clsPmpaType.IBR.Bohum / 100.0);      //진찰료
                clsPmpaType.RPG.Amt5[2] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[2] * clsPmpaType.IBR.Bohum / 100.0);      //입원료
                clsPmpaType.RPG.Amt5[4] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[4] * clsPmpaType.IBR.Bohum / 100.0);      //투약료     4
                clsPmpaType.RPG.Amt5[5] = (long)Math.Truncate((clsPmpaType.RPG.Amt1[5] - nAmtDrug11) * clsPmpaType.IBR.Bohum / 100.0);      //투약료2
                clsPmpaType.RPG.Amt5[6] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[6] * clsPmpaType.IBR.Bohum / 100.0);      //주사료2
                clsPmpaType.RPG.Amt5[7] = (long)Math.Truncate((clsPmpaType.RPG.Amt1[7] - nAmtDrug20) * clsPmpaType.IBR.Bohum / 100.0);      //주사료     7
                clsPmpaType.RPG.Amt5[8] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[8] * clsPmpaType.IBR.Bohum / 100.0);      //마취료
                clsPmpaType.RPG.Amt5[9] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[9] * clsPmpaType.IBR.Bohum / 100.0);      //처치료
                clsPmpaType.RPG.Amt5[10] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[10] * clsPmpaType.IBR.Bohum / 100.0);    //검사료     10
                clsPmpaType.RPG.Amt5[11] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[11] * clsPmpaType.IBR.Bohum / 100.0);    //영상진단
                clsPmpaType.RPG.Amt5[12] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[12] * clsPmpaType.IBR.Bohum / 100.0);    //방사선치료
                clsPmpaType.RPG.Amt5[13] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[13] * clsPmpaType.IBR.Bohum / 100.0);    //치료재료대 13
                clsPmpaType.RPG.Amt5[14] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[14] * clsPmpaType.IBR.Bohum / 100.0);    //물리치료
                clsPmpaType.RPG.Amt5[15] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[15] * clsPmpaType.IBR.Bohum / 100.0);    //정신요법
                clsPmpaType.RPG.Amt5[16] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[16] * clsPmpaType.IBR.Bohum / 100.0);    //수혈료     16
                clsPmpaType.RPG.Amt5[17] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[17] * clsPmpaType.IBR.CTMRI / 100.0);    //CT
                clsPmpaType.RPG.Amt5[18] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[18] * clsPmpaType.IBR.CTMRI / 100.0);    //MR
                clsPmpaType.RPG.Amt5[19] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[19] * clsPmpaType.IBR.Bohum / 100.0);    //초음파
                clsPmpaType.RPG.Amt5[20] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[20] * clsPmpaType.IBR.Bohum / 100.0);    //보철교정
                clsPmpaType.RPG.Amt5[49] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[49] * clsPmpaType.IBR.Bohum / 100.0);    //기타       49
                clsPmpaType.RPG.Amt5[2] = clsPmpaType.RPG.Amt5[2] + (long)Math.Truncate(nAMT09 * 25 / 100.0);
                clsPmpaType.RPG.Amt5[2] = clsPmpaType.RPG.Amt5[2] + (long)Math.Truncate(nAMT85 * 30 / 100.0);
                clsPmpaType.RPG.Amt5[2] = clsPmpaType.RPG.Amt5[2] + nKekliAmt_Bon + nHRoomBonin;
                clsPmpaType.RPG.Amt1[2] = clsPmpaType.RPG.Amt1[2] + nAMT09 + nAMT85 + nKekliAmt + nHRoomAmt;

            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// Description : RPG.AMT 변수에 본인부담금 세팅
        /// Description : 연말정산용으로 다시 만듬
        /// Author : 박웅규
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <seealso cref="IUMENT.bas : IRPG_AMT_SET"/>
        public void Ipd_RPG_Amt_Set_Junsan(PsmhDb pDbCon, int ArgBonRate, string ArgCt)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0;
            long nAmt = 0;
            long nAMT09 = 0;
            long nAMT85 = 0;
            long nAMT09_H = 0;
            long nAMT85_H = 0;
            long nKekliAmt = 0;
            long nKekliAmt_Bon = 0;
            long nHRoomAmt = 0;
            long nHRoomBonin = 0;
            int nBonRate = 0;

            nBonRate = ArgBonRate;

            #region //입원료 본인부담 선별대상 구분 2016-07-14
            if (string.Compare(clsPmpaType.TIT.M_InDate, clsPmpaPb.GstrLngRtDate) >= 0 && clsPmpaType.TIT.FCode != "F014" && VB.Left(clsPmpaType.TIT.Bi, 1) == "1" && clsPmpaType.TIT.VCode.Trim() == "" && clsPmpaType.TIT.OgPdBun.Trim() == "")
            {
                SQL = "";
                SQL += ComNum.VBLF + "  SELECT a.QTY,SUM(a.AMT1) Amt ";
                SQL += ComNum.VBLF + "    From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                SQL += ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_SUN b ";
                SQL += ComNum.VBLF + "   Where a.TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SQL += ComNum.VBLF + "     AND a.SUNEXT = b.SUNEXT(+)  ";
                SQL += ComNum.VBLF + "     AND b.DTLBUN = '1100' ";
                SQL += ComNum.VBLF + "     AND a.QTY <= 0.9 ";
                SQL += ComNum.VBLF + "     AND a.QTY >= 0.85  ";
                SQL += ComNum.VBLF + "     AND a.SUNEXT not in ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B','AV820A','AV820B','AV8201A','AV8201B') ";
                SQL += ComNum.VBLF + "   GROUP By a.QTY ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        if (Dt.Rows[i]["Amt"].ToString() != "")
                        {
                            nAmt = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());
                        }

                        if (Convert.ToDouble(Dt.Rows[i]["QTY"].ToString()) == 0.9)
                        {
                            nAMT09 = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString()); //본인부담 25%
                        }
                        else
                        {
                            nAMT85 = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString()); //본인부담 30%
                        }

                        clsPmpaType.RPG.Amt1[2] -= nAmt;
                    }
                }
                Dt.Dispose();
                Dt = null;
            }

            if (string.Compare(clsPmpaType.TIT.M_InDate,"2020-01-01") >= 0 && clsPmpaType.TIT.FCode != "F014" && VB.Left(clsPmpaType.TIT.Bi, 1) == "1" && clsPmpaType.TIT.VCode.Trim() == "" && clsPmpaType.TIT.OgPdBun.Trim() == "")
            {
                SQL = "";
                SQL += ComNum.VBLF + "  SELECT a.QTY,SUM(a.AMT1) Amt ";
                SQL += ComNum.VBLF + "    From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                SQL += ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_SUN b ";
                SQL += ComNum.VBLF + "   Where a.TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SQL += ComNum.VBLF + "     AND a.SUNEXT = b.SUNEXT(+)  ";
                SQL += ComNum.VBLF + "     AND b.DTLBUN = '1100' ";
                SQL += ComNum.VBLF + "     AND a.QTY <= 0.9 ";
                SQL += ComNum.VBLF + "     AND a.QTY >= 0.85  ";
                SQL += ComNum.VBLF + "     AND a.SUNEXT  in ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B') ";
                SQL += ComNum.VBLF + "   GROUP By a.QTY ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        if (Dt.Rows[i]["Amt"].ToString() != "")
                        {
                            nAmt = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());
                        }

                        if (Convert.ToDouble(Dt.Rows[i]["QTY"].ToString()) == 0.9)
                        {
                            nAMT09_H = (long)Math.Truncate(nAmt * 5 / 100.0);   //본인부담 5%가산   
                        }
                        else
                        {
                            nAMT85_H = (long)Math.Truncate(nAmt * 10 / 100.0);   //본인부담 10%가산   
                        }

                    }
                }
                Dt.Dispose();
                Dt = null;
            }

            #endregion //입원료 본인부담 선별대상 구분 2016-07-14

            #region //격리병실료 산정        '2016-09-22
            nKekliAmt = 0;
            nKekliAmt_Bon = 0;
            nAmt = 0;
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0 && ArgBonRate > 0)
            {
                SQL = "       SELECT /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1) KekliAmt ";
                SQL = SQL + ComNum.VBLF + "   FROM IPD_NEW_SLIP ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + clsPmpaType.TIT.Pano + "' ";
                SQL = SQL + ComNum.VBLF + "    AND TRSNO=" + clsPmpaType.TIT.Trsno + " ";
                SQL = SQL + ComNum.VBLF + "    AND SUCode IN ('AJ010','AJ020','AK200','AK201','AK202','AK210','AK211','V6001','V6002','AK200A','AH001','AH002') ";// 'AK200A  2017-07-18 ADD
                SQL = SQL + ComNum.VBLF + "    AND BDate>=TO_DATE('2016-09-23','YYYY-MM-DD') ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        nAmt = (long)VB.Val(Dt.Rows[i]["KekliAmt"].ToString().Trim()); 
                        nKekliAmt = nKekliAmt + nAmt;
                        if (clsPmpaType.TIT.VCode != "")
                        {
                            if (clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13")
                            {
                                if (clsPmpaType.TIT.VCode == "V000"  || clsPmpaType.TIT.VCode == "V010")
                                {
                                    nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * 0 / 100));
                                }
                                else
                                {
                                    nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * clsPmpaType.IBR.Bohum / 100.0));
                                }
                               
                            }
                        }
                        else
                        {
                            if (clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13" )
                            {
                                //F자격 제외 실제로는 본인부담없음
                                if (clsPmpaType.TIT.OgPdBun == "E" && clsPmpaType.TIT.Age < 6)
                                {
                                    //nKekliAmt_Bon = nKekliAmt_Bon + nAmt * (0 / 100);
                                }
                                if (clsPmpaType.TIT.OgPdBun == "E" && clsPmpaType.TIT.Age <= 15)
                                {
                                    nKekliAmt_Bon = nKekliAmt_Bon + nAmt * (3 / 100);
                                }
                                else if(clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2" || clsPmpaType.TIT.OgPdBun == "V" || clsPmpaType.TIT.OgPdBun == "H" || clsPmpaType.TIT.OgPdBun == "S" || clsPmpaType.TIT.OgPdBun == "Y")
                                {
                                    nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * 5 / 100));
                                }
                                else if (clsPmpaType.TIT.OgPdBun == "F" || clsPmpaType.TIT.OgPdBun == "C" || clsPmpaType.TIT.OgPdBun == "P")
                                {
                                    //nKekliAmt_Bon = nKekliAmt_Bon + nAmt * (0 / 100);
                                }
                                else
                                {
                                    nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * 10 / 100));
                                }
                            }
                            else if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.OgPdBun == "P")
                            {
                              //  nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * 0 / 100));
                            }
                            else if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.OgPdBun == "Y")
                            {
                                nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * 3 / 100));
                            }
                            else if (clsPmpaType.TIT.Bi == "22")
                            {
                                //2021-06-17
                                nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * 10 / 100));
                                //nKekliAmt_Bon = nKekliAmt_Bon + (long)Math.Round((double)(nAmt * 5 / 100));
                            }
                        }
                        clsPmpaType.RPG.Amt1[2] = clsPmpaType.RPG.Amt1[2] - nAmt;
                    }
                }
                Dt.Dispose();
                Dt = null;
            }
            #endregion //격리병실료 산정        '2016-09-22

            #region //2인병실료 본인부담 40%
            nHRoomAmt = 0; nHRoomBonin = 0; nAmt = 0;
            if (string.Compare(clsPmpaType.TIT.Bi ,"30") < 0)
            {
                SQL = "       SELECT /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1) HRoomAmt  ";
                SQL = SQL + ComNum.VBLF + "   FROM IPD_NEW_SLIP ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + clsPmpaType.TIT.Pano + "' ";
                SQL = SQL + ComNum.VBLF + "    AND TRSNO=" + clsPmpaType.TIT.Trsno + " ";
                SQL = SQL + ComNum.VBLF + "    AND SUCode IN ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B','AV820A','AV820B','AV8201A','AV8201B') ";// 'AK200A  2017-07-18 ADD
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        nAmt = (long)VB.Val(Dt.Rows[i]["HRoomAmt"].ToString().Trim());
                        nHRoomAmt = nHRoomAmt + nAmt;
                        nHRoomBonin = nHRoomBonin + (long)Math.Round((double)(nAmt * 40 / 100));

                        clsPmpaType.RPG.Amt1[2] = clsPmpaType.RPG.Amt1[2] - nAmt;
                    }
                    nHRoomBonin = nHRoomBonin + nAMT09_H + nAMT85_H;
                }
                Dt.Dispose();
                Dt = null;
            }
            #endregion //2인병실료 본인부담 40%

            clsPmpaType.RPG.Amt5[1] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[1] * nBonRate / 100));      //진찰료
            clsPmpaType.RPG.Amt5[2] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[2] * nBonRate / 100));      //입원료
            clsPmpaType.RPG.Amt5[4] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[4] * nBonRate / 100));      //투약료     4
            clsPmpaType.RPG.Amt5[5] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[5] * nBonRate / 100));      //투약료2
            clsPmpaType.RPG.Amt5[6] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[6] * nBonRate / 100));      //주사료2
            clsPmpaType.RPG.Amt5[7] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[7] * nBonRate / 100));      //주사료     7
            clsPmpaType.RPG.Amt5[8] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[8] * nBonRate / 100));      //마취료
            clsPmpaType.RPG.Amt5[9] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[9] * nBonRate / 100));      //처치료
            clsPmpaType.RPG.Amt5[10] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[10] * nBonRate / 100));    //검사료     10
            clsPmpaType.RPG.Amt5[11] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[11] * nBonRate / 100));    //영상진단
            clsPmpaType.RPG.Amt5[12] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[12] * nBonRate / 100));    //방사선치료
            clsPmpaType.RPG.Amt5[13] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[13] * nBonRate / 100));    //치료재료대 13
            clsPmpaType.RPG.Amt5[14] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[14] * nBonRate / 100));    //물리치료
            clsPmpaType.RPG.Amt5[15] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[15] * nBonRate / 100));    //정신요법
            clsPmpaType.RPG.Amt5[16] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[16] * nBonRate / 100));    //수혈료     16
            clsPmpaType.RPG.Amt5[19] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[19] * nBonRate / 100));    //초음파
            clsPmpaType.RPG.Amt5[20] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[20] * nBonRate / 100));    //보철교정
            clsPmpaType.RPG.Amt5[49] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[49] * nBonRate / 100));    //기타       49

            clsPmpaType.RPG.Amt5[2] = clsPmpaType.RPG.Amt5[2] + (long)Math.Round(((double)nAMT09 * 25 / 100));
            clsPmpaType.RPG.Amt5[2] = clsPmpaType.RPG.Amt5[2] + (long)Math.Round(((double)nAMT85 * 30 / 100));
            clsPmpaType.RPG.Amt5[2] = clsPmpaType.RPG.Amt5[2] + nKekliAmt_Bon + nHRoomBonin;
            clsPmpaType.RPG.Amt1[2] = clsPmpaType.RPG.Amt1[2] + nAMT09 + nAMT85 + nKekliAmt + nHRoomAmt;

            if (ArgCt != "")
            {
                clsPmpaType.RPG.Amt5[17] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[17] * nBonRate / 100));    //CT
                clsPmpaType.RPG.Amt5[18] = (long)Math.Round(((double)clsPmpaType.RPG.Amt1[18] * nBonRate / 100));    //MR
            }

        }

        /// <summary>
        /// Description : 입원마스타의 TRSCNT, LastTrs를 Update
        ///               IPD_TRANS의 InDate,OutDate를 재정리
        /// Author : 김민철
        /// Create Date : 2017.10.16
        /// </summary>
        /// <param name="ArgIpdNo"></param>
        /// <seealso cref="IUMENT.bas : IPD_TRANS_UPDATE"/>
        public void Ipd_Trans_Update(PsmhDb pDbCon, long ArgIpdNo)
        {
            int i = 0, nRead = 0;
            string strInDate = string.Empty;
            string strOutDate = string.Empty;
            string strGbIPD = string.Empty;
            string strM_InDate = string.Empty;
            string strM_OutDate = string.Empty;
            string strLastBi = string.Empty;

            int nIlsu = 0;
            long nTRSNO = 0;

            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;


            ComFunc CF = new ComFunc();

            try
            {
                //입원마스타의 입원일,퇴원일을 읽음
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(InDate,'YYYY-MM-DD') InDate, TO_CHAR(OutDate,'YYYY-MM-DD') OutDate ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + "  WHERE IPDNO=" + ArgIpdNo + " ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    strM_InDate = Dt.Rows[0]["InDate"].ToString().Trim();
                    strM_OutDate = Dt.Rows[0]["OutDate"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                clsDB.setBeginTran(pDbCon);

                //IPD_TRANS의 건수를 계산
                SQL = "";
                SQL += ComNum.VBLF + "SELECT GbIPD,TRSNO,Bi,TO_CHAR(InDate,'YYYY-MM-DD') InDate,";
                SQL += ComNum.VBLF + "       TO_CHAR(OutDate,'YYYY-MM-DD') OutDate,ILSU,ROWID ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                SQL += ComNum.VBLF + " WHERE IPDNO=" + ArgIpdNo + " ";
                SQL += ComNum.VBLF + "   AND GbIPD IN ('1','9') ";
                SQL += ComNum.VBLF + " ORDER BY GbIPD,InDate DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                nRead = Dt.Rows.Count;

                if (nRead > 0)
                {
                    nTRSNO = Convert.ToInt64(Dt.Rows[0]["TRSNO"].ToString());

                    for (i = 0; i < nRead; i++)
                    {
                        if (strGbIPD != Dt.Rows[i]["GBIPD"].ToString().Trim())
                        {
                            strOutDate = "";
                        }

                        //최종 보험종류를 저장
                        if (strLastBi == "")
                        {
                            if (Dt.Rows[i]["GbIPD"].ToString().Trim() == "1")
                            {
                                strLastBi = Dt.Rows[i]["Bi"].ToString().Trim();
                            }
                        }

                        //퇴원자는 마지막일자를 퇴원일자 SET
                        if (strOutDate == "")
                        {
                            if (strM_OutDate != "")
                                strOutDate = strM_OutDate;
                        }

                        strInDate = Dt.Rows[i]["InDate"].ToString().Trim();

                        if (strOutDate == "")
                        {
                            nIlsu = CF.DATE_ILSU(pDbCon, clsPublic.GstrSysDate, strInDate);
                        }
                        else
                        {
                            nIlsu = CF.DATE_ILSU(pDbCon, strOutDate, strInDate);
                        }

                        //자격별로 시작일,종료일을 Setting
                        SQL = "";
                        SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET ";
                        if (Dt.Rows[i]["GbIPD"].ToString().Trim() != "9" || clsPmpaPb.GstrAcctJob == "ICUPDT")  //구분변경시는 퇴원일자 세팅됨- 공용모듈
                        {
                            SQL += ComNum.VBLF + " OutDate=TO_DATE('" + strOutDate + "','YYYY-MM-DD'), ";     //지병건은 퇴원일자 세팅함..2010-01-13
                        }
                        
                        SQL += ComNum.VBLF + " Ilsu=" + nIlsu + " ";
                        SQL += ComNum.VBLF + "WHERE ROWID = '" + Dt.Rows[i]["ROWID"].ToString().Trim() + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        strOutDate = CF.DATE_ADD(pDbCon, Dt.Rows[i]["InDate"].ToString().Trim(), -1);
                        strGbIPD = Dt.Rows[i]["GbIPD"].ToString().Trim();
                    }
                }

                Dt.Dispose();
                Dt = null;

                //IPD_MASTER에 TRANS 갯수를 UPDATE
                if (clsPmpaPb.GstrChk == "2")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER  ";
                    SQL += ComNum.VBLF + "   SET Bi='" + strLastBi + "',";
                    SQL += ComNum.VBLF + "       TRSCNT=" + nRead + ",";
                    SQL += ComNum.VBLF + "       LASTTRS=" + nTRSNO + " ";
                    SQL += ComNum.VBLF + "       WHERE IPDNO=" + ArgIpdNo + " ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                clsDB.setCommitTran(pDbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        /// <summary>
        /// Description : 자격 기간예외값 발생점검
        /// Author : 김민철
        /// Create Date : 2018.02.12
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <param name="nTRSNO"></param>
        /// <param name="ArgInDate"></param>
        /// <param name="ArgOutDate"></param>
        public void Ipd_Trans_Amt_Check(PsmhDb pDbCon, string ArgPano, long nTRSNO, string ArgInDate, string ArgOutDate)
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = "";
            string strMsg = string.Empty;
            int i = 0;
            long nSumAmt = 0;

            SQL = "";
            SQL += " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,SUM(Amt1) Total                                   \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                                  \r\n";
            SQL += "  WHERE PANO = '" + ArgPano + "'                                                            \r\n";
            SQL += "    AND TRSNO =" + nTRSNO + "                                                               \r\n";
            SQL += "    AND (BDATE > TO_DATE('" + ArgOutDate + "','YYYY-MM-DD')                                 \r\n";
            SQL += "     OR BDATE < TO_DATE('" + ArgInDate + "','YYYY-MM-DD') )                                 \r\n";
            SQL += "    AND TRIM(SUNEXT) NOT IN (                                                               \r\n";
            SQL += "        SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE WHERE GUBUN ='원무영수제외코드' )  \r\n";
            SQL += "  GROUP BY BDATE                                                                                ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt64(VB.Val(dt.Rows[0]["TOTAL"].ToString())) != 0)
                {
                    strMsg = "이환자의 자격에서 기간외의 금액이 발생했습니다." + ComNum.VBLF + ComNum.VBLF;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strMsg += dt.Rows[i]["BDATE"].ToString().Trim() + " " + dt.Rows[i]["TOTAL"].ToString().Trim() + "원" + ComNum.VBLF;
                    nSumAmt += Convert.ToInt64(VB.Val(dt.Rows[0]["TOTAL"].ToString()));
                }

                if (nSumAmt != 0)
                {
                    strMsg += ComNum.VBLF + "반드시 입원SLIP을 확인해주세요";
                    ComFunc.MsgBox(strMsg, "확인요망");
                }
            }

            dt.Dispose();
            dt = null;

        }

        /// <summary>
        /// Description : IPD_TRANS을 읽어 금액을 TIT.Amt(01) ~ TIT.Amt(60)에 보관함
        /// ex: IPD_TRANS_AMT_READ(0,12324) -> IPD_TRANS TRSNO=12324의 금액을 표시
        ///     IPD_TRANS_AMT_READ(12345,0) -> IPD_TRANS IPDNO=12345 합계 금액을 표시
        /// Author : 김민철
        /// Create Date : 2018.02.12
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgIpdNo"></param> 
        /// <param name="ArgTrsNo"></param>
        /// <seealso cref="IUMENT.bas :IPD_TRANS_Amt_READ "/>
        public void Ipd_Trans_Amt_Read(PsmhDb pDbCon, long ArgIpdNo, long ArgTrsNo, string ArgGbSts)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int i = 0, j = 0, nRead = 0;
            long nTot1 = 0, nTot2 = 0;

            //누적할 배열 변수를 CLear
            for (i = 0; i < 92; i++)
            {
                clsPmpaType.TIT.Amt[i] = 0;
            }

            clsPmpaType.TIT.TotGub = 0;
            clsPmpaType.TIT.TotBigub = 0;

            #region //누적별 금액을 합산함
            SQL = "";
            SQL += " SELECT Bi,TO_CHAR(InDate,'YYYY-MM-DD') InDate,                         ";
            SQL += "        Amt01,Amt02,Amt03,Amt04,Amt05,Amt06,Amt07,Amt08,Amt09,Amt10,    ";
            SQL += "        Amt11,Amt12,Amt13,Amt14,Amt15,Amt16,Amt17,Amt18,Amt19,Amt20,    ";
            SQL += "        Amt21,Amt22,Amt23,Amt24,Amt25,Amt26,Amt27,Amt28,Amt29,Amt30,    ";
            SQL += "        Amt31,Amt32,Amt33,Amt34,Amt35,Amt36,Amt37,Amt38,Amt39,Amt40,    ";
            SQL += "        Amt41,Amt42,Amt43,Amt44,Amt45,Amt46,Amt47,Amt48,Amt49,Amt50,    ";
            SQL += "        Amt51,Amt52,Amt53,Amt54,Amt55,Amt56,Amt57,Amt58,Amt59,Amt60,    ";
            SQL += "        Amt61,Amt62,Amt63,Amt64,Amt65,Amt66,Amt67,Amt68,Amt69,Amt70,    ";
            SQL += "        Amt71,Amt72,Amt73,Amt74,Amt75,Amt76,Amt77,Amt78,Amt79,Amt80,    ";
            SQL += "        Amt81,Amt82,Amt83,Amt84,Amt85,Amt86,Amt87,Amt88,Amt89,Amt90,    ";
            SQL += "        Amt91                                                           ";
            SQL += "   FROM " + ComNum.DB_PMPA + "IPD_TRANS                                 ";
            if (ArgTrsNo == 0)
            {
                SQL += "WHERE IPDNO = " + ArgIpdNo + "                                      ";
            }
            else
            {
                SQL += "WHERE TrsNo = " + ArgTrsNo + "                                       ";
            }
            SQL += " ORDER BY InDate,Bi                                                     ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                for (j = 1; j < 92; j++)
                {
                    clsPmpaType.TIT.Amt[j] += Convert.ToInt64(VB.Val(dt.Rows[i]["AMT" + j.ToString("00")].ToString()));
                }
            }

            dt.Dispose();
            dt = null;
            #endregion

            #region //보증금,중간납 입금액
            if (string.Compare(ArgGbSts, "7") < 0 && ArgTrsNo == 0)
            {
                clsPmpaType.TIT.Amt[51] = 0;
                clsPmpaType.TIT.Amt[52] = 0;
                SQL = "";
                SQL += " SELECT SuNext,SUM(Amt) Amt                     \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH      \r\n";
                SQL += "  WHERE IPDNO=" + ArgIpdNo + "                  \r\n";
                SQL += "    AND SuNext IN ('Y85','Y87','Y88')           \r\n";
                SQL += "  GROUP BY SuNext                               \r\n";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["SUNEXT"].ToString().Trim() == "Y88")
                    {
                        if (dt.Rows[i]["Amt"].ToString().Trim() != "")
                        {
                            clsPmpaType.TIT.Amt[51] += Convert.ToInt64(VB.Val(dt.Rows[i]["Amt"].ToString()));
                        }
                    }
                    else
                    {
                        if (dt.Rows[i]["Amt"].ToString().Trim() != "")
                        {
                            clsPmpaType.TIT.Amt[52] += Convert.ToInt64(VB.Val(dt.Rows[i]["Amt"].ToString()));
                        }
                    }
                }

                dt.Dispose();
                dt = null;

            }
            #endregion

            #region//-----------( 급여, 비급여 합계금액을 계산 )---------------
            for (i = 0; i < 50; i++)
            {
                if (i > 0 && i < 21)
                {
                    clsPmpaType.TIT.TotGub += clsPmpaType.TIT.Amt[i];
                }
                else
                {
                    clsPmpaType.TIT.TotBigub += clsPmpaType.TIT.Amt[i];
                }
            }
            #endregion
        }

        /// <summary>
        /// 중간수납금액 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgIpdNo"></param>
        /// <returns></returns>
        public long IPD_Junggan_JanAmt(PsmhDb pDbCon, long ArgIpdNo)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            long rtnVal = 0;
            int i = 0;

            try
            {
                SQL = "";
                SQL += " SELECT SuNext,SUM(Amt) Amt ";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH  ";
                SQL += "  WHERE IPDNO=" + ArgIpdNo + " ";
                SQL += "    AND SuNext IN ('Y85','Y87','Y88') ";
                SQL += "  GROUP BY SuNext,TRSNO ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["SUNEXT"].ToString().Trim())
                        {
                            case "Y85":
                            case "Y87": rtnVal += Convert.ToInt64(VB.Val(dt.Rows[i]["AMT"].ToString())); break;     //입금
                            case "Y88": rtnVal -= Convert.ToInt64(VB.Val(dt.Rows[i]["AMT"].ToString())); break;     //대체
                        }
                    }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public void Ipd_Slip_Bon_Rate(PsmhDb pDbCon, clsPmpaType.ISBR ISBR, string GbSelfCheck = "")
        {
            //GBSELFCHECK PANO = 11391618, TRSNO = 1493306  환자 비급여 표시 관련하여 임시 처리

            int nBonRate = 0;
            long nBAMT = 0, nJAMT = 0, nFAMT = 0, nGAMT = 0;

            if (ISBR.SUNEXT.Trim() == "RAMNOS")
            {
                ISBR.SUNEXT = ISBR.SUNEXT;
            }

            if (ISBR.DTLBUN == "1100" && ISBR.QTY <= 0.9 && ISBR.QTY >= 0.85 && VB.Mid(ISBR.SUNEXT, 1, 5) != "AB270")       //장기입원료
            {
                #region 장기입원료 본인부담율
                if (ISBR.FCODE != "F014" && ISBR.VCODE.Trim() == "" && ISBR.OGPDBUN.Trim() == "" && VB.Left(ISBR.BI, 1) == "1")
                {
                    if (ISBR.QTY == 0.9)
                    {
                        nBonRate = 25;
                    }
                    else if (ISBR.QTY == 0.85)
                    {
                        nBonRate = 30;
                    }
                    else
                    {
                        nBonRate = clsPmpaType.IBR.Bohum;
                    }
                }
                else
                {
                    nBonRate = clsPmpaType.IBR.Bohum;
                }

                if (ISBR.BOHUN == "3")
                {
                    if ((VB.Left(ISBR.BI, 1) == "1" && ISBR.OGPDBUN == "F") || ISBR.BI == "22")
                    {
                        nBonRate = 0;
                    }
                }

                nBAMT = (long)Math.Truncate(ISBR.AMT * (nBonRate / 100.0));
                nGAMT = ISBR.AMT - nBAMT;
                #endregion
            }
            else if (ISBR.DTLBUN == "4003")
            {
                nBAMT = (long)Math.Truncate(ISBR.AMT * (clsPmpaType.IBR.Dt1 / 100.0));
                nGAMT = ISBR.AMT - nBAMT;
            }
            else if (ISBR.DTLBUN == "4004")
            {
                nBAMT = (long)Math.Truncate(ISBR.AMT * (clsPmpaType.IBR.Dt2 / 100.0));
                nGAMT = ISBR.AMT - nBAMT;
            }
            //주석해제!
            else if (ISBR.SUNEXT == "ID110" || ISBR.SUNEXT == "ID120" || ISBR.SUNEXT == "ID130")  //2021-09-16 결핵상담료, 관리료 본인부담금 없음.
            {
                nBAMT = 0;
                nGAMT = ISBR.AMT;
            }
            else if (ISBR.SUNEXT == "IA213" && string.Compare(ISBR.BDATE, "2020-11-01") >= 0)
            {
                nBAMT = (long)Math.Truncate(ISBR.AMT * (30 / 100.0));
                nGAMT = ISBR.AMT - nBAMT;
            }

            else if (ISBR.SUNEXT == "IA213" || ISBR.SUNEXT == "IA221" || ISBR.SUNEXT == "IA231" || ISBR.SUNEXT == "IA110" || ISBR.SUNEXT == "IA120")
            {
                nBAMT = 0;
                nGAMT = ISBR.AMT;
            }
            else if (ISBR.SUNEXT == "AB270" || ISBR.SUNEXT == "AB2701" || ISBR.SUNEXT == "AB270A" || ISBR.SUNEXT == "AB2701A"
                  || ISBR.SUNEXT == "AO280" || ISBR.SUNEXT == "AO2801" || ISBR.SUNEXT == "AV8201" || ISBR.SUNEXT == "AV8201A"
                  || ISBR.SUNEXT == "AV820" || ISBR.SUNEXT == "AO280A" || ISBR.SUNEXT == "AO2801A" || ISBR.SUNEXT == "AV8201B"
                  || ISBR.SUNEXT == "AO2801B" || ISBR.SUNEXT == "AO2801B" || ISBR.SUNEXT == "AV820A" || ISBR.SUNEXT == "AV820B")
            {
                if (ISBR.GBDRG != "D" && ((VB.Left(ISBR.BI, 1) == "1" || VB.Left(ISBR.BI, 1) == "2")))
                {


                    nBAMT = (long)Math.Truncate(ISBR.AMT * (40 / 100.0));
                    nGAMT = ISBR.AMT - nBAMT;
                }

            }

            else if ((ISBR.SUNEXT == "AJ010" || ISBR.SUNEXT == "AJ020" || ISBR.SUNEXT == "AK200" || ISBR.SUNEXT == "AK201" || ISBR.SUNEXT == "AK202"
                 || ISBR.SUNEXT == "AK210" || ISBR.SUNEXT == "AK211" || ISBR.SUNEXT == "V6001" || ISBR.SUNEXT == "V6002" || ISBR.SUNEXT == "AK200A" || ISBR.SUNEXT == "AH001" || ISBR.SUNEXT == "AH002") && (ISBR.GBDRG != "D"))
            {
                if (ISBR.VCODE != "")
                {
                    if (ISBR.BI == "11" || ISBR.BI == "12" || ISBR.BI == "13")
                    {
                        if (ISBR.VCODE == "V000" || ISBR.VCODE == "V010")
                        {
                            nBAMT = (long)Math.Truncate(ISBR.AMT * (0 / 100.0));
                            nGAMT = ISBR.AMT - nBAMT;

                        }
                        else
                        {
                            nBAMT = (long)Math.Truncate(ISBR.AMT * (clsPmpaType.IBR.Jin / 100.0));
                            nGAMT = ISBR.AMT - nBAMT;
                        }

                    }
                }
                else
                {
                    if (ISBR.BI == "11" || ISBR.BI == "12" || ISBR.BI == "13")
                    {
                        //F자격제외 실제로는 본인부담 없음
                        if (ISBR.OGPDBUN == "E" && clsPmpaType.TIT.Age < 6)
                        {
                            nBAMT = (long)Math.Truncate(ISBR.AMT * (0 / 100.0));
                            nGAMT = ISBR.AMT - nBAMT;
                        }
                        else if (ISBR.OGPDBUN == "E" && clsPmpaType.TIT.Age <= 15)
                        {
                            nBAMT = (long)Math.Truncate(ISBR.AMT * (3 / 100.0));
                            nGAMT = ISBR.AMT - nBAMT;
                        }
                        else if (ISBR.OGPDBUN == "E" || ISBR.OGPDBUN == "1" || ISBR.OGPDBUN == "2" || ISBR.OGPDBUN == "V" || ISBR.OGPDBUN == "H" || ISBR.OGPDBUN == "S" || ISBR.OGPDBUN == "Y")
                        {
                            nBAMT = (long)Math.Truncate(ISBR.AMT * (5 / 100.0));
                            nGAMT = ISBR.AMT - nBAMT;
                        }
                        else if (ISBR.OGPDBUN == "F" || ISBR.OGPDBUN == "C")
                        {
                            nBAMT = (long)Math.Truncate(ISBR.AMT * (0 / 100.0));
                            nGAMT = ISBR.AMT - nBAMT;
                        }
                        else
                        {
                            nBAMT = (long)Math.Truncate(ISBR.AMT * (10 / 100.0));
                            nGAMT = ISBR.AMT - nBAMT;
                        }
                    }
                    else if (ISBR.BI == "22" && ISBR.OGPDBUN == "P")
                    {
                        nBAMT = (long)Math.Truncate(ISBR.AMT * (0 / 100.0));
                        nGAMT = ISBR.AMT - nBAMT;
                    }
                    else if (ISBR.BI == "22" && ISBR.OGPDBUN == "Y")
                    {
                        nBAMT = (long)Math.Truncate(ISBR.AMT * (3 / 100.0));
                        nGAMT = ISBR.AMT - nBAMT;
                    }
                    else if (ISBR.BI == "22")
                    {
                        //2021-06-17 의뢰서
                        nBAMT = (long)Math.Truncate(ISBR.AMT * (10 / 100.0));
                        //nBAMT = (long)Math.Truncate(ISBR.AMT * (5 / 100.0));
                        nGAMT = ISBR.AMT - nBAMT;
                    }
                    else
                    {
                        nBAMT = (long)Math.Truncate(ISBR.AMT * (clsPmpaType.IBR.Bohum / 100.0));
                        nGAMT = ISBR.AMT - nBAMT;
                    }
                }

            }

            else if ((ISBR.SUNEXT == "Y1110" || ISBR.SUNEXT == "T1110" || ISBR.SUNEXT == "Z4200" || ISBR.SUNEXT == "Z0100"
                 || ISBR.SUNEXT == "Z0011" || ISBR.SUNEXT == "Z0010" || ISBR.SUNEXT == "Z0020") && (ISBR.GBDRG != "D") && (ISBR.NU <= 20))
            {
                if (ISBR.OGPDBUN == "P" || ISBR.OGPDBUN == "C" || ISBR.OGPDBUN == "E" || ISBR.OGPDBUN == "F" || ISBR.OGPDBUN == "1" || ISBR.OGPDBUN == "2")
                {
                    nBAMT = (long)Math.Truncate(ISBR.AMT * (0 / 100.0));
                    nGAMT = ISBR.AMT - nBAMT;
                }
                else if (ISBR.BI == "11" || ISBR.BI == "12" || ISBR.BI == "13")

                {
                    nBAMT = (long)Math.Truncate(ISBR.AMT * (50 / 100.0));
                    nGAMT = ISBR.AMT - nBAMT;
                }
                else

                {
                    nBAMT = (long)Math.Truncate(ISBR.AMT * (clsPmpaType.IBR.Bohum / 100.0));
                    nGAMT = ISBR.AMT - nBAMT;
                }
            }

            else if (ISBR.SUNEXT == "IA213" && string.Compare(ISBR.BDATE, "2020-11-01") >= 0)
            {
                nBAMT = (long)Math.Truncate(ISBR.AMT * (30 / 100.0));
                nGAMT = ISBR.AMT - nBAMT;
            }


            else if (ISBR.SUNEXT == "IA213" || ISBR.SUNEXT == "IA221" || ISBR.SUNEXT == "IA231" || ISBR.SUNEXT == "IA110" || ISBR.SUNEXT == "IA120")
            {
                if (ISBR.GBDRG != "D")
                {
                    nBAMT = 0;
                    nGAMT = ISBR.AMT;
                }

            }
            else
            {
                #region 나머지 ...

                if (GbSelfCheck == "OK" && (ISBR.GBSELF == "1" || ISBR.GBSELF == "2"))
                {
                    if (ISBR.GBSELF == "2" && ISBR.GBSUGBS == "1")
                    {
                        nJAMT = ISBR.AMT;       //전액본인부담
                    }
                    else
                    {
                        nFAMT = ISBR.AMT;       //비급여
                    }
                    
                }
                else
                {
                    //본인부담율 표시
                    if ((ISBR.SUGBP == "0" || ISBR.SUGBP == "2" || ISBR.SUGBP == "9") && ISBR.GBSUGBS != "0" && ISBR.NU >= 21 && ISBR.GBSELF == "2")
                    {
                        if (ISBR.GBDRG == "D")
                        {
                            if (ISBR.DRGF != "" || ISBR.DRG100 != "")
                            {
                                nJAMT = ISBR.AMT;       //전액본인부담
                            }
                        }
                        else
                        {
                            nJAMT = ISBR.AMT;       //전액본인부담
                        }
                    }
                    else if (ISBR.NU >= 21)
                    {
                        if (ISBR.GBDRG == "D")
                        {
                            if (ISBR.DRGF != "" || ISBR.DRG100 != "")
                            {
                                nFAMT = ISBR.AMT;       //전액본인부담
                            }
                        }
                        else
                        {
                            nFAMT = ISBR.AMT;       //비급여
                        }
                    }
                    else
                    {
                        #region 선별수가별 본인부담율 Set
                        if (ISBR.GBSUGBS == "3" || ISBR.GBSUGBS == "4" || ISBR.GBSUGBS == "5" || ISBR.GBSUGBS == "6" || ISBR.GBSUGBS == "7" || ISBR.GBSUGBS == "8" || ISBR.GBSUGBS == "9")
                        {
                            nBonRate = Bon_Rate_100(ISBR.GBSUGBS);

                            if (ISBR.BI == "52" || ISBR.BI == "31")
                            {
                                nBonRate = 0;
                            }
                        }
                        else
                        {
                            nBonRate = 0;
                        }
                        #endregion

                        if (nBonRate > 0)   //선별급여
                        {
                            #region 선별급여
                            if (ISBR.BOHUN == "3")  //장애인
                            {
                                if ((VB.Left(ISBR.BI, 1) == "1" && ISBR.OGPDBUN == "F") || ISBR.BI == "22")
                                {
                                    nBonRate = 0;
                                    nGAMT = ISBR.AMT;
                                }
                            }
                            else
                            {
                                if (ISBR.GBDRG != "D")
                                {
                                    nBAMT = (long)Math.Truncate(ISBR.AMT * (nBonRate / 100.0));
                                    nGAMT = ISBR.AMT - nBAMT;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region 분류별 본인부담
                            if (ISBR.BUN == "01" || ISBR.BUN == "02")
                            {
                                if (ISBR.GBDRG != "D")
                                {
                                    nBAMT = (long)Math.Truncate(ISBR.AMT * (clsPmpaType.IBR.Jin / 100.0));
                                    nGAMT = ISBR.AMT - nBAMT;
                                }
                            }
                            else if (ISBR.BUN == "72" || ISBR.BUN == "73")
                            {
                                if (ISBR.GBDRG != "D")
                                {
                                    nBAMT = (long)Math.Truncate(ISBR.AMT * (clsPmpaType.IBR.CTMRI / 100.0));
                                    nGAMT = ISBR.AMT - nBAMT;
                                }
                            }
                            else if (ISBR.BUN == "74")
                            {
                                if (ISBR.GBDRG != "D")
                                {
                                    nBAMT = (long)Math.Truncate(ISBR.AMT * (clsPmpaType.IBR.Food / 100.0));
                                    nGAMT = ISBR.AMT - nBAMT;
                                }
                            }
                            else
                            {
                                if (ISBR.GBDRG != "D")
                                {
                                    nBAMT = (long)Math.Truncate(ISBR.AMT * (clsPmpaType.IBR.Bohum / 100.0));
                                    nGAMT = ISBR.AMT - nBAMT;
                                }
                            }
                            #endregion

                            #region 장애인
                            if (ISBR.BOHUN == "3")  //장애인
                            {
                                if ((VB.Left(ISBR.BI, 1) == "1" && ISBR.OGPDBUN == "F") || ISBR.BI == "22")
                                {
                                    #region 분류별 본인부담
                                    if (ISBR.BUN == "01" || ISBR.BUN == "02")
                                    {
                                        nGAMT = ISBR.AMT;
                                    }
                                    else if (ISBR.BUN == "72" || ISBR.BUN == "73")
                                    {
                                        nBAMT = (long)Math.Truncate(ISBR.AMT * (clsPmpaType.IBR.CTMRI / 100.0));
                                        nGAMT = ISBR.AMT - nBAMT;
                                    }
                                    else if (ISBR.BUN == "74")
                                    {
                                        nBAMT = (long)Math.Truncate(ISBR.AMT * (clsPmpaType.IBR.Food / 100.0));
                                        nGAMT = ISBR.AMT - nBAMT;
                                    }
                                    else
                                    {
                                        nGAMT = ISBR.AMT;
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                        }

                    }
                }
                #endregion
            }
            

            ISBR.nBBAmt[0] = nBAMT;   //본인부담
            ISBR.nBGAmt[0] = nGAMT;   //공단부담
            ISBR.nBJAmt[0] = nJAMT;   //전액부담
            ISBR.nBFAmt[0] = nFAMT;   //비급여

            ISBR.nBBAmt[1] += ISBR.nBBAmt[0];
            ISBR.nBGAmt[1] += ISBR.nBGAmt[0];
            ISBR.nBJAmt[1] += ISBR.nBJAmt[0];
            ISBR.nBFAmt[1] += ISBR.nBFAmt[0];

        }

        int Bon_Rate_100(string strGbn)
        {
            int rtnRate = 0;

            switch (strGbn)
            {
                case "1": rtnRate = 100; break;
                case "4": rtnRate = 80; break;
                case "5": rtnRate = 50; break;
                case "6": rtnRate = 80; break;
                case "7": rtnRate = 50; break;
                case "8": rtnRate = 90; break;
                case "9": rtnRate = 90; break;
                default:
                    rtnRate = 0;
                    break;
            }
            return rtnRate;
        }
        
    }
}
