using Store.Domain.Contracts;
using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Specifications
{
    public class BaseSpecifications<TKey, TEntity> : ISpecifications<TKey, TEntity> where TEntity : BaseEntity<TKey>
    {
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }

        public Expression<Func<TEntity, object>>? OrderBy { get ; set; }
        public Expression<Func<TEntity, object>>? OrderByDescending { get; set; }
        public int Skip { get ; set; }
        public int Take { get; set; }
        public bool IsPagination { get; set; }

        public BaseSpecifications(Expression<Func<TEntity,bool>>? expression)
        {
            Criteria = expression;
        }
        public void ApplyPagination(int pagesize,int pageIndex)
        {
            IsPagination = true;
            Skip = (pageIndex - 1) * pagesize;
            Take = pagesize;
        }
        public void AddOrderBy(Expression<Func<TEntity, object>>? expression)
        {
            OrderBy = expression;
        }

            public void AddOrderByDescending(Expression<Func<TEntity, object>>? expression)
        {
            OrderByDescending = expression;
        }
    }
}
