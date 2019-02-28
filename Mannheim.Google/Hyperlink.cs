using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Google
{
    public class Hyperlink
    {
        public Uri Url { get; set; }

        private string title;
        public string Title
        {
            get
            {
                return this.title ?? this.Url.AbsoluteUri;
            }
            set
            {
                this.title = value;
            }
        }


    }
}
