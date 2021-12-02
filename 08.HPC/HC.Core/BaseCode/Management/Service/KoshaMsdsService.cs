using ComBase.Mvc.Utils;
using HC.Core.BaseCode.Management.Model;
using HC.Core.BaseCode.MSDS.Dto;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HC.Core.BaseCode.Management.Service
{
    /// <summary>
    /// Kosha open api
    /// </summary>
    public class KoshaMsdsService
    {
     
        string serviceKey = "rCX%2BNeuRBFI%2BQL1nZZJ%2BdfZ5Ue2jqKVMhRWELOxsY0w02puA56c8XQpklGlySvzsFneHmIPe4Z11dxmhkqSZiA%3D%3D";
        string hostUrl = "http://msds.kosha.or.kr/openapi/service/msdschem";
        /// <summary>
        /// kosha 물질 검색
        /// </summary>
        /// <param name="isName"></param>
        /// <param name="searchWord"></param>
        /// <returns></returns>
        public List<KoshaMsds> SearchKoshaMsds(bool isName, string searchWord)
        {
            RestClient client = new RestClient(hostUrl);
            client.Encoding = Encoding.UTF8;


            RestRequest request = new RestRequest("chemlist", Method.GET);
            request.AddHeader("Content-Type", "application/xml;charset=utf-8");

            if (isName) {
                request.AddParameter("searchCnd", "0");
            }
            else
            {
                request.AddParameter("searchCnd", "1");
            }
            request.AddParameter("searchWrd", searchWord);
            request.AddParameter("ServiceKey", serviceKey, ParameterType.QueryStringWithoutEncode);


            var response = client.Get(request);

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(response.Content);
            XmlNodeList nodes = xml.SelectNodes("/response/body/items/item");

            List<KoshaMsds> list = new List<KoshaMsds>();
            foreach (XmlNode node in nodes)
            {
                KoshaMsds model = new KoshaMsds();
                model.ChemId = node.SelectSingleNode("chemId").InnerText;
                model.CasNo = node.SelectSingleNode("casNo").InnerText;
                model.ChemNameKor = node.SelectSingleNode("chemNameKor").InnerText;
                list.Add(model);
            }
            return list;

        }

        /// <summary>
        /// kosha 물질 법규정보
        /// http://msds.kosha.or.kr/openapi/service/msdschem/chemdetail15?chemId=001008&ServiceKey=rCX%2BNeuRBFI%2BQL1nZZJ%2BdfZ5Ue2jqKVMhRWELOxsY0w02puA56c8XQpklGlySvzsFneHmIPe4Z11dxmhkqSZiA%3D%3D
        /// </summary>
        /// <param name="isName"></param>
        /// <param name="searchWord"></param>
        /// <returns></returns>
        public HC_MSDS GetKoshaRule(string chemdId)
        {
            RestClient client = new RestClient(hostUrl);
            client.Encoding = Encoding.UTF8;


            RestRequest request = new RestRequest("chemdetail15", Method.GET);
            request.AddHeader("Content-Type", "application/xml;charset=utf-8");
            request.AddParameter("chemId", chemdId);

            request.AddParameter("ServiceKey", serviceKey, ParameterType.QueryStringWithoutEncode);

            var response = client.Get(request);

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(response.Content);
            XmlNodeList nodes = xml.SelectNodes("/response/body/items/item");
            string itemDetail =  nodes[0].SelectSingleNode("itemDetail").InnerText;

            HC_MSDS dto = new HC_MSDS();
            string[] rules = itemDetail.Split('|');
            foreach(string rule in rules)
            {
                if (rule.Contains("노출기준설정물질"))
                {
                    dto.EXPOSURE_MATERIAL = "Y";
                }
                else if (rule.Contains("작업환경"))
                {
                    string[] tmp = rule.Split(':');
                    if (tmp.Length > 1)
                    {
                        string period = tmp[tmp.Length - 1];
                        period = period.Substring(0, period.Length - 1);

                        dto.WEM_MATERIAL = period;
                    }
                    else
                    {
                        dto.WEM_MATERIAL = "Y";
                    }
                }
                else if (rule.Contains("관리대상"))
                {
                    string[] tmp = rule.Split(':');
                    if (tmp.Length > 1)
                    {
                        string period = tmp[tmp.Length - 1];
                        period = period.Substring(0, period.Length - 1);

                        dto.MANAGETARGET_MATERIAL = period;
                    }
                    else
                    {
                        dto.MANAGETARGET_MATERIAL = "Y";
                    }
                }
                else if (rule.Contains("특수건강"))
                {
                    string[] tmp = rule.Split(':');
                    if (tmp.Length > 1)
                    {
                        string period = tmp[tmp.Length - 1];
                        period = period.Substring(0, period.Length - 1);

                        dto.SPECIALHEALTH_MATERIAL = period;
                    }
                    else
                    {
                        dto.SPECIALHEALTH_MATERIAL = "Y";
                    }
                }
                else if (rule.Contains("특별관리"))
                {
                    string[] tmp = rule.Split(':');
                    if (tmp.Length >   1)
                    {
                        string period = tmp[tmp.Length - 1];
                        period = period.Substring(0, period.Length - 1);

                        dto.SPECIALMANAGE_MATERIAL = period;
                    }
                    else
                    {
                        dto.SPECIALMANAGE_MATERIAL = "Y";
                    }
                }
                else if (rule.Contains("공정안전"))
                {
                    string[] tmp = rule.Split(':');
                    if (tmp.Length > 1)
                    {
                        string period = tmp[tmp.Length - 1];
                        period = period.Substring(0, period.Length - 1);

                        dto.PSM_MATERIAL = period;
                    }
                    else
                    {
                        dto.PSM_MATERIAL = "Y";
                    }
                }
                else if (rule.Contains("노출기준"))
                {
                    string[] tmp = rule.Split(':');
                    if (tmp.Length > 1)
                    {
                        string period = tmp[tmp.Length - 1];
                        period = period.Substring(0, period.Length - 1);

                        dto.EXPOSURE_MATERIAL = period;
                    }
                    else
                    {
                        dto.EXPOSURE_MATERIAL = "Y";
                    }
                }
                else if (rule.Contains("허용기준"))
                {
                    string[] tmp = rule.Split(':');
                    if (tmp.Length > 1)
                    {
                        string period = tmp[tmp.Length - 1];
                        period = period.Substring(0, period.Length - 1);

                        dto.STANDARD_MATERIAL = period;
                    }
                    else
                    {
                        dto.STANDARD_MATERIAL = "Y";
                    }
                }
                else if (rule.Contains("허가"))
                {
                    string[] tmp = rule.Split(':');
                    if (tmp.Length > 1)
                    {
                        string period = tmp[tmp.Length - 1];
                        period = period.Substring(0, period.Length - 1);

                        dto.PERMISSION_MATERIAL = period;
                    }
                    else
                    {
                        dto.PERMISSION_MATERIAL = "Y";
                    }
                }
            }

            return dto;

        }
        public string GetGHSPicture(string chemdId)
        {
            RestClient client = new RestClient(hostUrl);
            client.Encoding = Encoding.UTF8;


            RestRequest request = new RestRequest("chemdetail02", Method.GET);
            request.AddHeader("Content-Type", "application/xml;charset=utf-8");
            request.AddParameter("chemId", chemdId);

            request.AddParameter("ServiceKey", serviceKey, ParameterType.QueryStringWithoutEncode);

            var response = client.Get(request);

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(response.Content);
            XmlNodeList nodes = xml.SelectNodes("/response/body/items/item");
            string ghsString = string.Empty;
            foreach (XmlNode node in nodes)
            {
                if (node.SelectSingleNode("msdsItemNameKor").InnerText.Equals("그림문자"))
                {
                    ghsString = node.SelectSingleNode("itemDetail").InnerText;
                }
            }

            return ghsString;
            
        }
    }
}
