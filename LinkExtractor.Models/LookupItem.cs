﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkExtractor.Models
{
    public class LookupItem
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }

    }
    public class NullLookupItem : LookupItem
    {
        public new int? Id { get { return null; } }
    }
}
