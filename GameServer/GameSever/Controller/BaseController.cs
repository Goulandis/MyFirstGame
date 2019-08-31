using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameSever.Servers;

namespace GameSever.Controller
{
    //所有Controller类的基类
    class BaseController
    {
        protected RequestCode requestCode = RequestCode.None;

        public RequestCode RequestCode
        {
            get { return requestCode; }
        }

        public virtual string DefaultHandle(string data, Client client, Server server)
        {
            return null;
        }
    }
}
