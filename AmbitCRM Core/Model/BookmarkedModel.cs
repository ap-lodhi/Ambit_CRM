using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbitCRM_Core.Model
{
    public class BookmarkedModel
    {
        public int BookMarkedId { get; set; }
        public string UserId { get; set; }
        public int ContactId { get; set; }
        public bool IsBookMarked { get; set; }
    }


  }
