using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using CoreWCF;

namespace PhraseFinder.WCF.Services;

[DataContract]
public class EchoMessage
{
    [DataMember]
    [AllowNull]
    public string Text { get; set; }
}

[DataContract]
public class EchoFault
{
    [DataMember]
    [AllowNull]
    public string Text { get; set; }
}

[ServiceContract]
public interface IEchoService
{
    [OperationContract]
    string Echo(string text);

    [OperationContract]
    string ComplexEcho(EchoMessage message);

    [OperationContract]
    [FaultContract(typeof(EchoFault))]
    string FailEcho(string text);
}
