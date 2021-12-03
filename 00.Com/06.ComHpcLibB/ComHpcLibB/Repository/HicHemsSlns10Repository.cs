using System;
using System.Collections.Generic;
using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;

namespace ComHpcLibB.Repository
{
    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HicHemsSlns10Repository : BaseRepository
    {
        public List<object> FindAll(string id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT * FROM TABLE              ");
            parameter.AppendSql(" WHERE ID = :id                   ");

            parameter.Add("id", id);

            return ExecuteReader<object>(parameter);
        }

        public void Save(object obj)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO TABLE           ");
            parameter.AppendSql(" ( USERID, NAME             )");
            parameter.AppendSql(" VALUES                      ");
            parameter.AppendSql(" (:userId, :name            )");

            parameter.Add("userId", obj);
            parameter.Add("name", obj);

            ExecuteNonQuery(parameter);
        }
    }
}
