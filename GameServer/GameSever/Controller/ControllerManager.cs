using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Reflection;
using GameSever.Servers;

namespace GameSever.Controller
{
    //管理所有Controller类，根据客户端传入的不同的RequestCode寻找对应的Controller类进行处理
    class ControllerManager
    {
        //不同的请求对应不同的处理脚本，存储此一个映射的字典
        private Dictionary<RequestCode, BaseController> controllerManager = new Dictionary<RequestCode, BaseController>();
        private Server server;

        public ControllerManager(Server server)
        {
            this.server = server;
            InitController();
        }

        void InitController()//初始化Dictionary<RequestCode, BaseController>字典，映射的添加需要手动添加
        {
            controllerManager.Add(RequestCode.None, new DefaultController());
            controllerManager.Add(RequestCode.User, new UserController());
            controllerManager.Add(RequestCode.Room, new RoomController());
            controllerManager.Add(RequestCode.Game, new GameController());
        }

        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data,Client client)
        {
            BaseController controller;//所有Controller类的基类，用于针对不同的请求引用不同的Controller类
            bool isGet = controllerManager.TryGetValue(requestCode, out controller);//判断Dictionary<RequestCode, BaseController>中是否存在对应的脚本
            if (isGet == false)
            {
                Console.WriteLine("无法获得[" + requestCode + "]对应的controller，无法处理请求");
            }
            string methodName = Enum.GetName(typeof(ActionCode), actionCode);//获取ActionCode枚举类中对应的枚举成员
            MethodInfo mi = controller.GetType().GetMethod(methodName);//获取controller引用的处理脚本中与methodName名字对应的方法信息
            if (mi == null)
            {
                Console.WriteLine("[警告]在controller[" + controller.GetType() + "]中没有对应的处理方法:[" + methodName + "]");
                return;
            }
            object[] parameters = new object[] { data,client,server };//打包MethodInfo所需要传递的参数
            object o = mi.Invoke(controller, parameters);//调用mi所获取的方法，处理对应的请求消息，返回一个包含调用方法返回值的对象
            if (o == null || string.IsNullOrEmpty(o as string))//如果没有对应的方法终止函数
            {
                return;
            }
            server.SendResponse(client, actionCode, o as string);//开始发送反馈消息
        }
    }
}
