using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Cube;
using AutoMapper;
using Hakone.Domain;
using MvcPaging;
using Nest;
using Hakone.Domain.Enum;

namespace Hakone.Service
{
    public partial class ShopService : IShopService
    {
        private readonly ElasticClient _client = new ElasticClientWrapper().GetClient();
        public IPagedList<ShopES> GetPagedList(string listFor, int? catId, string keyword, int page, string orderby = "", string city = "", int pagesize = 100)
        {
            var query = new QueryContainer(new NumericRangeQuery { Field = "shopViews", GreaterThanOrEqualTo = 0 });

            var sorts = new List<ISort>();

            if (catId.HasValue)
            {
                query = query && new TermQuery { Field = "catId", Value = catId };
            }
            if (keyword.IsNotNullOrEmpty())
            {
                query = query && (new MatchQuery { Field = "shopName", Query = keyword, Operator = Nest.Operator.And }
                    || new MatchQuery { Field = "shopTags", Query = keyword, Operator = Nest.Operator.And }
                    || new MatchQuery { Field = "mainBiz", Query = keyword, Operator = Nest.Operator.And });
            }
            if (city.IsNotNullOrEmpty())
            {
                query = query && new MatchQuery { Field = "city", Query = city, Operator = Nest.Operator.And };
            }

            var enumListFor = listFor.ToEnum<ShopListFor>(true);
            switch (enumListFor)
            {
                case ShopListFor.recommend:
                    query = query && new TermQuery { Field = "isRecommend", Value = true };
                    sorts.Add(new SortField { Field = "recommendDate", Order = SortOrder.Descending });
                    break;
                case ShopListFor.hot:
                    query = query && new TermQuery { Field = "isHot", Value = true };
                    sorts.Add(new SortField { Field = "hotDate", Order = SortOrder.Descending });
                    break;
                case ShopListFor.selected:
                    query = query && new TermQuery { Field = "isSelected", Value = true };
                    sorts.Add(new SortField { Field = "selectedDate", Order = SortOrder.Descending });
                    break;
                case ShopListFor.nor:
                    query = query && new TermQuery { Field = "isRecommend", Value = false };
                    sorts.Add(new SortField { Field = "lastModifyDate", Order = SortOrder.Descending });
                    break;
                default:
                    sorts.Add(new SortField { Field = "recommendDate", Order = SortOrder.Descending });
                    break;
            }

            

            if (orderby.IsNotNullOrEmpty())
            {
                sorts.Clear();
                var shopOrderby = @orderby.ToEnum<ShopOrderBy>();
                switch (shopOrderby)
                {
                    case ShopOrderBy.ByCollectionDate:
                        sorts.Add(new SortField { Field = "lastCollectionDate", Order = SortOrder.Descending });
                        break;
                    case ShopOrderBy.ByRecommendDate:
                        sorts.Add(new SortField { Field = "recommendDate", Order = SortOrder.Descending });
                        break;
                    case ShopOrderBy.EntryDate:
                        sorts.Add(new SortField { Field = "entryDate", Order = SortOrder.Descending });
                        break;
                    case ShopOrderBy.PromoteAccount:
                        sorts.Add(new SortField { Field = "promoteAccount", Order = SortOrder.Descending });
                        break;
                    case ShopOrderBy.PromoteAmount:
                        sorts.Add(new SortField { Field = "promoteAmount", Order = SortOrder.Descending });
                        sorts.Add(new SortField { Field = "promoteAccount", Order = SortOrder.Descending });
                        break;
                    case ShopOrderBy.UpdateDate:
                        sorts.Add(new SortField { Field = "lastModifyDate", Order = SortOrder.Descending });
                        break;
                    case ShopOrderBy.ByDefault:
                        sorts.Add(new SortField { Field = "lastModifyDate", Order = SortOrder.Descending });
                        break;
                    default:
                        sorts.Add(new SortField { Field = "lastModifyDate", Order = SortOrder.Descending });
                        break;
                }
            }
            else
            {
                sorts.Add(new SortField { Field = "lastModifyDate", Order = SortOrder.Descending });
            }

            var request = new SearchRequest("haodian8", Types.Type(typeof(ShopES)))
            {
                From = pagesize * page,
                Size = pagesize,
                Query = query,
                Sort = sorts
            };

            var response = _client.Search<ShopES>(request);

            //var list = Mapper.Map<List<ShopES>, List<Shop>>(response.Documents.ToList());

            var countRequest = new CountRequest("haodian8", Types.Type(typeof(ShopES)))
            {
                Query = query
            };
            var countResponse = _client.Count<ShopES>(countRequest);
            var pagingTotalCount = countResponse.Count > 19980 ? 19980 : (int)countResponse.Count;

            return response.Documents.ToPagedList(page, pagesize, pagingTotalCount);

        }

        public List<ShopES> GetLatestSelectedShops()
        {
            return GetTopShops("isSelected", "selectedDate", 6);
        }

        public List<ShopES> GetLatestRecommendShops()
        {
            return GetTopShops("isRecommend", "recommendDate", 12);
        }

        private List<ShopES> GetTopShops(string field, string fieldDate, int takeNumber)
        {
            var query = new QueryContainer(new TermQuery { Field = field, Value = true });

            var request = new SearchRequest("haodian8", Types.Type(typeof(ShopES)))
            {
                From = 0,
                Size = takeNumber,
                Query = query,
                Sort =new List<ISort>
                        {
                            new SortField { Field = fieldDate, Order = SortOrder.Descending },
                        }
            };

            var response = _client.Search<ShopES>(request);

            return response.Documents.ToList();
        }

        
    }
}
