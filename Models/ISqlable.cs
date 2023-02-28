using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp_ProjectV2.DataAccessLayer;

namespace ToDoApp_ProjectV2.Models
{
    public interface ISqlable
    {
        public void SqlCreate();
        public void SqlDelete();
        public void SqlUpdate()
        {
            this.SqlDelete();
            this.SqlCreate();
        }
    }

}
