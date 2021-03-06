﻿using Sif.Framework.Demo.Broker.Models;
using Sif.Framework.Service.Registration;
using Sif.Framework.Service.Serialisation;
using Sif.Framework.Utils;
using Sif.Framework.WebApi;
using Sif.Framework.WebApi.MediaTypeFormatters;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Xml.Serialization;

namespace Sif.Framework.Demo.Broker
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IRegistrationService registrationService;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // URL Postfix Extension: Update the configuration to recognise postfix extensions and map known
            // extensions to MIME Types. Additional changes to WebApiConfig.cs are required to fully enable this
            // feature.
            GlobalConfiguration.Configuration.Formatters.JsonFormatter
                .AddUriPathExtensionMapping("json", "application/json");
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.AddUriPathExtensionMapping("xml", "text/xml");

            // XML Serialisation: Define the specific XML serialiser to use to ensure that SIF Data Model Objects (as
            // defined by the SIF Specification with XML Schema Definitions (XSDs)) are serialised correctly.
            XmlMediaTypeFormatter formatter = GlobalConfiguration.Configuration.Formatters.XmlFormatter;
            formatter.UseXmlSerializer = true;

            // XML Serialisation: For each SIF Data Model Object used by each SIF Provider, the following entries are
            // required to define the root element for each collection object.
            var queuesXmlRootAttribute = new XmlRootAttribute("Queues")
            { Namespace = SettingsManager.ProviderSettings.InfrastructureNamespace, IsNullable = false };
            ISerialiser<List<Queue>> queuesSerialiser =
                SerialiserFactory.GetXmlSerialiser<List<Queue>>(queuesXmlRootAttribute);
            formatter.SetSerializer<List<Queue>>((XmlSerializer)queuesSerialiser);

            var subscriptionsXmlRootAttribute = new XmlRootAttribute("Subscriptions")
            { Namespace = SettingsManager.ProviderSettings.InfrastructureNamespace, IsNullable = false };
            ISerialiser<List<Subscription>> subscriptionsSerialiser =
                SerialiserFactory.GetXmlSerialiser<List<Subscription>>(subscriptionsXmlRootAttribute);
            formatter.SetSerializer<List<Subscription>>((XmlSerializer)subscriptionsSerialiser);

            var studentPersonalsXmlRootAttribute = new XmlRootAttribute("StudentPersonals")
            { Namespace = SettingsManager.ProviderSettings.DataModelNamespace, IsNullable = false };
            ISerialiser<List<StudentPersonal>> studentPersonalsSerialiser =
                SerialiserFactory.GetXmlSerialiser<List<StudentPersonal>>(studentPersonalsXmlRootAttribute);
            formatter.SetSerializer<List<StudentPersonal>>((XmlSerializer)studentPersonalsSerialiser);

            // Replacement custom JSON formatter (compliant with Goessner notation).
            var xmlToJsonFormatter = new XmlToJsonFormatter { UseXmlSerializer = true };
            xmlToJsonFormatter.AddUriPathExtensionMapping("json", "application/json");
            xmlToJsonFormatter.SetSerializer<List<Queue>>((XmlSerializer)queuesSerialiser);
            xmlToJsonFormatter.SetSerializer<List<Subscription>>((XmlSerializer)subscriptionsSerialiser);
            xmlToJsonFormatter.SetSerializer<List<StudentPersonal>>((XmlSerializer)studentPersonalsSerialiser);
            GlobalConfiguration.Configuration.Formatters.Add(xmlToJsonFormatter);
            GlobalConfiguration.Configuration.Formatters
                .Remove(GlobalConfiguration.Configuration.Formatters.JsonFormatter);

            // Configure global exception loggers for unexpected errors.
            GlobalConfiguration.Configuration.Services.Add(typeof(IExceptionLogger), new TraceExceptionLogger());

            // Configure a global exception handler for unexpected errors.
            GlobalConfiguration.Configuration.Services.Replace(typeof(IExceptionHandler),
                new GlobalUnexpectedExceptionHandler());

            Trace.TraceInformation("********** Application_Start **********");
            Register();
        }

        protected void Application_End(object sender, System.EventArgs e)
        {
            Trace.TraceInformation("********** Application_End **********");
            Unregister();
        }

        /// <summary>
        /// Register this SIF Provider with the EnvironmentProvider based upon settings defined in the SIF 3.0
        /// Framework configuration, e.g. SifFramework.config.
        /// </summary>
        private void Register()
        {
            registrationService = RegistrationManager.GetProviderRegistrationService(
                SettingsManager.ProviderSettings,
                SessionsManager.ProviderSessionService);
            registrationService.Register();
        }

        /// <summary>
        /// Unregister this SIF Provider from the EnvironmentProvider.
        /// </summary>
        private void Unregister()
        {
            registrationService.Unregister();
        }
    }
}