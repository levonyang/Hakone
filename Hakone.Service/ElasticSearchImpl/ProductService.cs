using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Domain;
using Hakone.Service;
using Hakone.Cube;
using MvcPaging;
using Hakone.Domain.Enum;
using Nest;
using AutoMapper;

namespace Hakone.Service
{
    public partial class ProductService : IProductService
    {
        private readonly ElasticClient _client = new ElasticClientWrapper().GetClient();

        public IPagedList<Product> GetList(int catId, string s, int r, int page)
        {
            var pageSize = 60;

            var query = new QueryContainer();

            if (catId > 0)
            {
                if (s.IsNotNullOrEmpty())
                {
                    var catName = ProductCategoryService.GetEntity(catId).CategoryName;
                    query = query && (new TermQuery {Field = "catId", Value = catId}
                                      || new MatchQuery { Field = "productName", Query = catName, Operator = Nest.Operator.And });
                }
                else
                {
                    query = query && (new TermQuery {Field = "catId", Value = catId});
                }
            }
            if(s.IsNotNullOrEmpty())
            {
                query = query && new MatchQuery { Field = "productName", Query = s, Operator = Nest.Operator.And };
                query = query && new NumericRangeQuery { Field = "amountSales", GreaterThan = 10 };
            }
            if (r == 1)
            {
                query = query && new TermQuery {Field = "isRecommend", Value = true};
            }
            else if (r == 0)
            {
                query = query && new NumericRangeQuery {Field = "price", GreaterThan = 1};
            }


            var countRequest = new CountRequest("haodian8", Types.Type(typeof(ProductES)))
            {
                Query = query
            };

            var countResponse = _client.Count<ProductES>(countRequest);
            var pagingTotalCount = countResponse.Count > 19980 ? 19980 : (int)countResponse.Count;

            var from = pageSize * page;
            if (catId == 0 && s.IsNullOrEmpty() && r==1)
            {
                from += 30;
                pagingTotalCount -= 30;
            }

            var request = new SearchRequest("haodian8",Types.Type(typeof(ProductES)))
            {
                From = from,
                Size = pageSize,
                Query = query,
                Sort =new List<ISort>
                        {
                            new SortField { Field = "isRecommend", Order = SortOrder.Descending },
                            new SortField { Field = "recommendDate", Order = SortOrder.Descending },
                            new SortField { Field = "isSelectedShop", Order = SortOrder.Descending },
                            new SortField { Field = "amountSales", Order = SortOrder.Descending },
                        }
            };

            

            var response = _client.Search<ProductES>(request);

            List<Product> list = Mapper.Map<List<ProductES>, List<Product>>(response.Documents.ToList());

            return list.ToPagedList(page, pageSize, pagingTotalCount);
        }
    }
}
