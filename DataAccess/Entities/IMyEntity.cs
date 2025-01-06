using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public interface IMyEntity<T> where T : class
    {
        public List<T> GetInitializationEntities();
        public string GetTableName();
        public string[] GetColumnsSql();
    }
}
