/* 作    者: xml
** 创建时间: 2024/5/26 16:44:54
**
** Copyright 2024 by zedmoster
** Permission to use, copy, modify, and distribute this software in
** object code form for any purpose and without fee is hereby granted,
** provided that the above copyright notice appears in all copies and
** that both that copyright notice and the limited warranty and
** restricted rights notice below appear in all supporting
** documentation.
*/

using Microsoft.Extensions.DependencyInjection;
using xml.Revit.Dev.DocumentBase.Services;
using xml.Revit.Dev.DocumentBase.StorageFactory;

namespace xml.Revit.Dev.DocumentBase
{
    /// <summary>
    /// DI
    /// </summary>
    public static class Host
    {
        private static ServiceProvider _serviceProvider;

        /// <summary>
        /// 加载启动
        /// </summary>
        static Host()
        {
            Start();
        }

        /// <summary>
        ///     Starts the host and configures the application's services.
        /// </summary>
        public static void Start()
        {
            var services = new ServiceCollection();

            services.AddScoped<StorageFactoryElement>();

            services.AddScoped<ILevelService, LevelService>();
            services.AddScoped<IGridService, GridService>();
            services.AddScoped<IViewPlanService, ViewPlanService>();
            services.AddScoped<IWallService, WallService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        ///     Gets a service of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <returns>A service object of type T or null if there is no such service.</returns>
        public static T GetService<T>()
            where T : class
        {
            return _serviceProvider.GetService(typeof(T)) as T;
        }
    }
}
