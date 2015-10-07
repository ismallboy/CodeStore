
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

namespace Amway.OA.ETOffine
{
    /// <summary>
    /// 服务容器
    /// </summary>
    public class ServiceLocator
    {
        /// <summary>
        /// 框架IOC容器名称
        /// </summary>
        public const string FrameworkContainerName = "AmwayFrameworkContainer";

        /// <summary>
        /// 用于线程同步的对象

        /// </summary>
        private static Object _lockObject = new Object();

        /// <summary>
        /// 框架IOC容器
        /// 只读
        /// </summary>
        private static IUnityContainer container;

        /// <summary>
        /// 空间IOC 容器
        /// </summary>
        public static IUnityContainer Container
        {
            get {
                //延迟加载
                //线程同步
                if (container == null)
                {
                    lock (_lockObject)
                    {
                        if (container == null)
                        {
                            InitUnityContainer();
                        }
                    }             
                }
                return ServiceLocator.container; 
            
            }
        }

        /// <summary>
        /// 初始化IOC容器
        /// </summary>
        public  static void InitUnityContainer()
        {
            IUnityContainer _container = new UnityContainer();
            _container.LoadConfiguration(FrameworkContainerName);

            var unityCommon = (UnityConfigurationSection)ConfigurationManager.GetSection("unityCommon");
            if (unityCommon != null)
            {
                unityCommon.Configure(_container, FrameworkContainerName); 
            }

            unityCommon = (UnityConfigurationSection)ConfigurationManager.GetSection("unityApp");
            if (unityCommon != null)
            {
                unityCommon.Configure(_container, FrameworkContainerName);
            }
            container = _container;
        }

        /// <summary>
        /// 从IOC容器中返回类型为T的组件

        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {
            return Container.Resolve<T>();
        }

        /// <summary>
        /// 从IOC容器中返回类型为T的组件中
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="name">组件登记名称</param>
        /// <returns>返回的组件</returns>
        public static T GetService<T>(string name)
        {
            return Container.Resolve<T>(name);
        }
    }
}
