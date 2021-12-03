namespace ComHpcLibB.Repository
{
    using ComBase;
    using ComBase.Mvc;
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HicBarcodeReqRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HicBarcodeReqRepository()
        {
        }

        public int HIC_BARCODE_REQ_Insert(long WRTNO, string strBuse)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_BARCODE_REQ    ");
            parameter.AppendSql("       (GBBUSE, WRTNO)                     ");
            parameter.AppendSql("VALUES                                     ");
            parameter.AppendSql("       (:GBBUSE, :WRTNO)                   ");

            parameter.Add("WRTNO", WRTNO);
            parameter.Add("GBBUSE", strBuse, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int HIC_BARCODE_REQ_Insert_HIC_His(long fnPano, string strJepDate, string idNumber, string gstrCOMIP)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_BARCODE_REQ_HIS                ");
            parameter.AppendSql("       (GBBUSE, WRTNO, INPDT, INPID, INPIP)                ");
            parameter.AppendSql("SELECT '1', WRTNO, SYSDATE, :INPID, :INPIP                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                               ");
            parameter.AppendSql(" WHERE PANO = :PANO                                        ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("   AND DELDATE IS NULL                                     ");

            parameter.Add("PANO", fnPano);
            parameter.Add("JEPDATE", strJepDate);
            parameter.Add("INPID", idNumber);
            parameter.Add("INPIP", gstrCOMIP);

            return ExecuteNonQuery(parameter);
        }

        public int HIC_BARCODE_REQ_Insert_His(long nWRTNO, string strGbBuse, string idNumber, string gstrCOMIP)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_BARCODE_REQ_HIS        ");
            parameter.AppendSql("       (GBBUSE, WRTNO, INPDT, INPID, INPIP)        ");
            parameter.AppendSql("VALUES                                             ");
            parameter.AppendSql("       (:GBBUSE, :WRTNO, SYSDATE, :INPID, :INPIP)  ");

            parameter.Add("GBBUSE", strGbBuse, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("INPID", idNumber);
            parameter.Add("INPIP", gstrCOMIP);

            return ExecuteNonQuery(parameter);
        }

        public int HIC_BARCODE_REQ_Insert_HIC(long fnPano, string strJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_BARCODE_REQ                    ");
            parameter.AppendSql("       (GBBUSE, WRTNO)                                     ");
            parameter.AppendSql("SELECT '1', WRTNO FROM KOSMOS_PMPA.HIC_JEPSU               ");
            parameter.AppendSql(" WHERE PANO = :PANO                                        ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("   AND DELDATE IS NULL                                     ");

            parameter.Add("PANO", fnPano);
            parameter.Add("JEPDATE", strJepDate);

            return ExecuteNonQuery(parameter);
        }
    }
}
