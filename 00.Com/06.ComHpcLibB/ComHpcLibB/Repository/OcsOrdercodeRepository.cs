namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class OcsOrdercodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public OcsOrdercodeRepository()
        {
        }

        public List<OCS_ORDERCODE> FindSlip(string argSlipNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COrderCode,OrderName,ItemCd,GbInfo,DispSpace,GbInput,GbDosage,CSuCode   ");
            parameter.AppendSql("  FROM " + ComNum.DB_MED + "OCS_ORDERCODE                                      ");
            parameter.AppendSql(" WHERE 1 = 1                                                                   ");
            parameter.AppendSql("   AND SlipNo =:SlipNo                                                        ");
            parameter.AppendSql("   AND SeqNo  > 0                                                              ");
            parameter.AppendSql("   AND (SendDept IS NULL OR SendDept <> 'N')                                   ");
            parameter.AppendSql(" ORDER BY SlipNo,SeqNo,OrderCode                                               ");

            parameter.Add("SlipNo", argSlipNo.Trim());

            return ExecuteReader<OCS_ORDERCODE>(parameter);
        }

        public OCS_ORDERCODE GetItembyOrderCode(string argOrderCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SLIPNO, SUCODE, ORDERNAME, BUN      ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_ORDERCODE            ");
            parameter.AppendSql(" WHERE ORDERCODE = :ORDERCODE              ");

            parameter.Add("ORDERCODE", argOrderCode.Trim(), Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<OCS_ORDERCODE>(parameter);
        }

        public string GetOrderCodebyItemCd(string fstrORDERCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ORDERCODE                   ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_ORDERCODE    ");
            parameter.AppendSql(" WHERE ITEMCD = :ITEMCD            ");

            parameter.Add("ITEMCD", fstrORDERCODE.Trim(), Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }
    }
}
