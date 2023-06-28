using Model.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ProductDAO
    {
        SuperMarketKDbContext db = null;
        public ProductDAO()
        {
            db = new SuperMarketKDbContext();
        }

        public List<Product> getList(int top)
        {
            return db.Products.Where(x=>x.Quantity > 0).OrderBy(x => x.CreatedDate).Take(top).ToList();
        }

        public List<Product> getListHot()
        {
            List<long> list = db.Database.SqlQuery<long>("select top 4 b.ProductID from [Order] a,[OrderDetail] b,[Product] c where a.ID = b.OrderID and b.ProductID = c.id group by b.ProductID  order by count(*) desc").ToList();
            return db.Products.Where(x=>list.Contains(x.ID)).ToList();
        }

        public List<Product> getRecommend()
        {
            return db.Products.Where(x => x.Quantity > 0).OrderBy(x => Guid.NewGuid()).Take(4).ToList();
        }

        public ProductCategory getCategory(string metatile)
        {
            return db.ProductCategories.Single(x => x.MetaTitle == metatile);
        }

        public List<Product> getProductByKeyword(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).ToList();
        }

        public List<Product> getProductByCategoryID(long id)
        {
            return db.Products.Where(x => x.CategoryID == id).ToList();
        }

        public List<Product> getProductByCateID(long idCurrent)
        {
            var idGroup = getProductByID(idCurrent);
            return db.Products.Where(x=>x.CategoryID == idGroup.CategoryID && x.ID != idCurrent).Take(4).ToList();
        }

        public Product getProductByID(long id)
        {
            return db.Products.Find(id);
        }

        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }

        public IEnumerable<Product> getAllPaging(string searchString, int page, int pageSize)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return db.Products.Where(x => x.Name.Contains(searchString)
                || x.Code.Contains(searchString)).OrderBy(x => x.ID).ToPagedList(page, pageSize);
            }
            return db.Products.OrderBy(x => x.ID).ToPagedList(page, pageSize);
        }
    }
}
