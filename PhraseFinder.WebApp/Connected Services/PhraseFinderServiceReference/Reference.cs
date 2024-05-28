﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PhraseFinderServiceReference
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PhraseAnalysis", Namespace="http://schemas.datacontract.org/2004/07/PhraseFinder.WCF.Contracts")]
    public partial class PhraseAnalysis : object
    {
        
        private PhraseFinderServiceReference.FoundPhrase[] FoundPhrasesField;
        
        private string ProcessedTextField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public PhraseFinderServiceReference.FoundPhrase[] FoundPhrases
        {
            get
            {
                return this.FoundPhrasesField;
            }
            set
            {
                this.FoundPhrasesField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ProcessedText
        {
            get
            {
                return this.ProcessedTextField;
            }
            set
            {
                this.ProcessedTextField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FoundPhrase", Namespace="http://schemas.datacontract.org/2004/07/PhraseFinder.WCF.Contracts")]
    public partial class FoundPhrase : object
    {
        
        private string BaseWordField;
        
        private PhraseFinderServiceReference.PhraseDefinition[] DefinitionsField;
        
        private int LengthField;
        
        private string MatchField;
        
        private string PhraseField;
        
        private int PhraseIdField;
        
        private int StartIndexField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string BaseWord
        {
            get
            {
                return this.BaseWordField;
            }
            set
            {
                this.BaseWordField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public PhraseFinderServiceReference.PhraseDefinition[] Definitions
        {
            get
            {
                return this.DefinitionsField;
            }
            set
            {
                this.DefinitionsField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Length
        {
            get
            {
                return this.LengthField;
            }
            set
            {
                this.LengthField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Match
        {
            get
            {
                return this.MatchField;
            }
            set
            {
                this.MatchField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Phrase
        {
            get
            {
                return this.PhraseField;
            }
            set
            {
                this.PhraseField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PhraseId
        {
            get
            {
                return this.PhraseIdField;
            }
            set
            {
                this.PhraseIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int StartIndex
        {
            get
            {
                return this.StartIndexField;
            }
            set
            {
                this.StartIndexField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PhraseDefinition", Namespace="http://schemas.datacontract.org/2004/07/PhraseFinder.WCF.Contracts")]
    public partial class PhraseDefinition : object
    {
        
        private string DefinitionField;
        
        private string[] ExamplesField;
        
        private int PhraseIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Definition
        {
            get
            {
                return this.DefinitionField;
            }
            set
            {
                this.DefinitionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Examples
        {
            get
            {
                return this.ExamplesField;
            }
            set
            {
                this.ExamplesField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PhraseId
        {
            get
            {
                return this.PhraseIdField;
            }
            set
            {
                this.PhraseIdField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PhraseFinderServiceReference.IPhraseFinderService")]
    public interface IPhraseFinderService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPhraseFinderService/FindPhrases", ReplyAction="http://tempuri.org/IPhraseFinderService/FindPhrasesResponse")]
        System.Threading.Tasks.Task<PhraseFinderServiceReference.PhraseAnalysis> FindPhrasesAsync(string text);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public interface IPhraseFinderServiceChannel : PhraseFinderServiceReference.IPhraseFinderService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public partial class PhraseFinderServiceClient : System.ServiceModel.ClientBase<PhraseFinderServiceReference.IPhraseFinderService>, PhraseFinderServiceReference.IPhraseFinderService
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public PhraseFinderServiceClient() : 
                base(PhraseFinderServiceClient.GetDefaultBinding(), PhraseFinderServiceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IPhraseFinderService.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public PhraseFinderServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(PhraseFinderServiceClient.GetBindingForEndpoint(endpointConfiguration), PhraseFinderServiceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public PhraseFinderServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(PhraseFinderServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public PhraseFinderServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(PhraseFinderServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public PhraseFinderServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<PhraseFinderServiceReference.PhraseAnalysis> FindPhrasesAsync(string text)
        {
            return base.Channel.FindPhrasesAsync(text);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IPhraseFinderService))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IPhraseFinderService))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost:54833/PhraseFinderService.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return PhraseFinderServiceClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IPhraseFinderService);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return PhraseFinderServiceClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IPhraseFinderService);
        }
        
        public enum EndpointConfiguration
        {
            
            BasicHttpBinding_IPhraseFinderService,
        }
    }
}
