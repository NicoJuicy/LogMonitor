﻿using System.Configuration;

namespace LogMonitor.Configuration.Ouput
{
    public class GraphiteElement : ConfigurationElement
    {
        /// <summary>
        /// The XML name of the <see cref="Address"/> property.
        /// </summary>        
        internal const string AddressPropertyName = "address";

        /// <summary>
        /// The XML name of the <see cref="Port"/> property.
        /// </summary>        
        internal const string PortPropertyName = "port";

        /// <summary>
        /// The XML name of the <see cref="Transport"/> property.
        /// </summary>        
        internal const string TransportPropertyName = "transport";

        /// <summary>
        /// Gets or sets the port number.
        /// </summary>        
        [ConfigurationPropertyAttribute(PortPropertyName, IsRequired = true)]
        public int Port
        {
            get { return (int)base[PortPropertyName]; }
            set { base[PortPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the port number.
        /// </summary>        
        [ConfigurationPropertyAttribute(AddressPropertyName, IsRequired = true)]
        public string Address
        {
            get { return (string)base[AddressPropertyName]; }
            set { base[AddressPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the port number.
        /// </summary>        
        [ConfigurationProperty(TransportPropertyName, IsRequired = true, DefaultValue = TransportType.Tcp)]
        public TransportType Transport
        {
            get { return (TransportType)base[TransportPropertyName]; }
            set { base[TransportPropertyName] = value; }
        }
    }
}
