﻿using System;
using FluentScheduler;
using Padawan.Startup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Newtonsoft.Json;

namespace Padawan.Extensions
{
    public static class WebHostBuilderExtensions
    {
        
        public static IWebHostBuilder UsePadawan<TStartup>(this IWebHostBuilder hostBuilder, string applicationName = null) where TStartup : class
        {

            var startupName = typeof(TStartup).GetTypeInfo().Assembly.GetName().Name;
            
            hostBuilder.UseSetting(WebHostDefaults.ApplicationKey, applicationName ?? startupName);

            hostBuilder.ConfigureServices(collection =>
            {
                // Register Padawan services by attributes
                collection.RegisterAttributes();

                // Register Padawan configurations by attributes
                collection.RegisterConfigurationAttributes();

                // Register Padawan Scheduler job by attributes
                collection.RegisterSchedulerJobAttributes();

                // Add configured application & Configurition of using of Padawan application 
                collection.AddTransient<IStartupFilter, StartupFilter>();

            });

            hostBuilder.UseStartup(startupName);

            return hostBuilder;

        }


        /// <summary>
        /// Use Daylight Saving Time
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static IWebHostBuilder UseUtcTime(this IWebHostBuilder hostBuilder) 
        {

            // Json Serializer Settings
            Padawan.JsonSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            //  FLuent Scheduler Timing
            JobManager.UseUtcTime();

            return hostBuilder;

        }

        /// <summary>
        /// Use Local Time
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static IWebHostBuilder UseLocalTime(this IWebHostBuilder hostBuilder)
        {

            // Json Serializer Settings
            Padawan.JsonSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            

            return hostBuilder;

        }


        /// <summary>
        /// Configure Padawan Json Option
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="options">Newtonsoft Json Serializer Settings</param>
        /// <returns></returns>
        public static IWebHostBuilder ConfigurePadawanJsonOption(this IWebHostBuilder hostBuilder, Action<JsonSerializerSettings> options)
        {

            options.Invoke(Padawan.JsonSerializerSettings);


            return hostBuilder;

        }
    }
}

