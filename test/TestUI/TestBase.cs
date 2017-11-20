using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUI
{
    // class intended to be the base for my test Classes, for consistent functional testing
    //
    public abstract class TestBase
    {
        public virtual String RunTests()
        {
            String classname = this.GetType().Name;

            return String.Format("class {0} - RunTests() not yet implemented", classname);
        }
    }

}
